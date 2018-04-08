﻿#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// Contributors:
//    Takeshi KIRIYA
//
#endregion -- License Terms --

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if DEBUG && !UNITY
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements serialization target member extraction logics.
	/// </summary>
	internal static class SerializationTarget
	{
		public static void VerifyType( Type targetType )
		{
			if ( targetType.GetIsInterface() || targetType.GetIsAbstract() )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( targetType );
			}
		}

		public static IList<SerializingMember> Prepare( SerializationContext context, Type targetType )
		{
			var result = PrepareCore( context, targetType );

			VerifyNilImplication( targetType, result );
			VerifyKeyUniqueness( result );

			return result;
		}

        
//20150611 applibot modify start 
        [AOT.MonoPInvokeCallback(typeof(Func<SerializingMember, int>))]
        public static int aotInt(SerializingMember item){
            return item.Contract.Id;
        }
        public static Func<SerializingMember, int> intDelegateForAOT_ = aotInt;
        
        //20150611 applibot modify  
        [AOT.MonoPInvokeCallback(typeof(Func<SerializingMember, string>))]
        public static string aotName(SerializingMember item){
            return item.Contract.Name;
        }
        public static Func<SerializingMember, string> nameDelegateForAOT_ = aotName;

        //20150611 applibot modify  
        [AOT.MonoPInvokeCallback(typeof(Func<SerializingMember, bool>))]
        public static bool aotBool(SerializingMember item){
            return item.Contract.Id == DataMemberContract.UnspecifiedId;
        }
        public static Func<SerializingMember, bool> boolDelegateForAOT_ = aotBool;
//20150611 applibot modify end


		private static IList<SerializingMember> PrepareCore( SerializationContext context, Type targetType )
		{
            //20150611 applibot modify 
            //var entries = GetTargetMembers( targetType ).OrderBy( item => item.Contract.Id ).ToArray();
            var entries = GetTargetMembers( targetType ).OrderBy( intDelegateForAOT_ ).ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( targetType );
			}
            
            //20150611 applibot modify 
            //if ( entries.All( item => item.Contract.Id == DataMemberContract.UnspecifiedId ) )
            if ( entries.All( boolDelegateForAOT_ ) )
			{
                // Alphabetical order.
                //20150611 applibot modify 
                //return entries.OrderBy( item => item.Contract.Name ).ToArray();
                return entries.OrderBy( nameDelegateForAOT_ ).ToArray();
			}

			// ID order.

#if DEBUG && !UNITY
			Contract.Assert( entries[ 0 ].Contract.Id >= 0 );
#endif // DEBUG && !UNITY

			if ( context.CompatibilityOptions.OneBoundDataMemberOrder && entries[ 0 ].Contract.Id == 0 )
			{
				throw new NotSupportedException( "Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true." );
			}
            
            //20150611 applibot modify 
            int maxId = 0;
            foreach(SerializingMember item in entries){
                if(maxId < item.Contract.Id){
                    maxId = item.Contract.Id;
                }
            }
            //var maxId = entries.Max( item => item.Contract.Id );
            //var maxId = entries.Max( intDelegateForAOT_ );

			var result = new List<SerializingMember>( maxId + 1 );
			for ( int source = 0, destination = context.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0; source < entries.Length; source++, destination++ )
			{
#if DEBUG && !UNITY
				Contract.Assert( entries[ source ].Contract.Id >= 0 );
#endif // DEBUG && !UNITY

				if ( entries[ source ].Contract.Id < destination )
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID '{0}' is duplicated in the '{1}' elementType.", entries[ source ].Contract.Id, targetType ) );
				}

				while ( entries[ source ].Contract.Id > destination )
				{
					result.Add( new SerializingMember() );
					destination++;
				}

				result.Add( entries[ source ] );
			}

			return result;
		}

		// internal for testing
		internal static IEnumerable<SerializingMember> GetTargetMembers( Type type )
		{
#if DEBUG && !UNITY
			Contract.Assert( type != null );
#endif // DEBUG && !UNITY
#if !NETFX_CORE
			var members =
				type.FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					( member, criteria ) => CheckTargetEligibility( member ),
					null
				);
			var filtered = members.Where( item => Attribute.IsDefined( item, typeof( MessagePackMemberAttribute ) ) ).ToArray();
#else
			var members =
				type.GetRuntimeFields().Where( f => !f.IsStatic ).OfType<MemberInfo>()
					.Concat( type.GetRuntimeProperties().Where( p => p.GetMethod != null && !p.GetMethod.IsStatic ) )
					.Where( CheckTargetEligibility )
					.ToArray();
			var filtered = members.Where( item => item.IsDefined( typeof( MessagePackMemberAttribute ) ) ).ToArray();
#endif

			if ( filtered.Length > 0 )
			{
				var duplicated =
					filtered.FirstOrDefault(
						member =>
#if !NETFX_CORE
							Attribute.IsDefined( member, typeof( MessagePackIgnoreAttribute ) )
#else
							member.IsDefined( typeof( MessagePackIgnoreAttribute ) ) 
#endif // !NETFX_CORE
					);

				if ( duplicated != null )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"A member '{0}' of type '{1}' is marked with both MessagePackMemberAttribute and MessagePackIgnoreAttribute.",
							duplicated.Name,
							type 
						)
					);
				}

				return
					filtered.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<MessagePackMemberAttribute>() )
						)
					);
			}

			if ( type.GetCustomAttributesData().Any( attr =>
				attr.GetAttributeType().FullName == "System.Runtime.Serialization.DataContractAttribute" ) )
			{
				return
					members.Select(
						item =>
						new
						{
							member = item,
							data = item.GetCustomAttributesData()
								.FirstOrDefault(
									data => data.GetAttributeType().FullName == "System.Runtime.Serialization.DataMemberAttribute" )
						}
					).Where( item => item.data != null )
					.Select(
						item =>
						{
							var name = item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Name" )
								.Select( arg => ( string )arg.GetTypedValue().Value )
								.FirstOrDefault();
							var id = item.data.GetNamedArguments()
								.Where( arg => arg.GetMemberName() == "Order" )
								.Select( arg => ( int? )arg.GetTypedValue().Value )
								.FirstOrDefault();
#if SILVERLIGHT
							if ( id == -1 )
							{
								// Shim for Silverlight returns -1 because GetNamedArguments() extension method cannot recognize whether the argument was actually specified or not.
								id = null;
							}
#endif // SILVERLIGHT

							return
								new SerializingMember(
									item.member,
									new DataMemberContract( item.member, name, NilImplication.MemberDefault, id )
								);
						}
					);
			}
			return
				members.Where(
					member => member.GetIsPublic()
#if !SILVERLIGHT && !NETFX_CORE
					&& !Attribute.IsDefined( member, typeof( NonSerializedAttribute ) )
#endif // !SILVERLIGHT && !NETFX_CORE
#if !NETFX_CORE
					&& !Attribute.IsDefined( member, typeof( MessagePackIgnoreAttribute ) )
#else
					&& !member.IsDefined( typeof( MessagePackIgnoreAttribute ) ) 
#endif // !NETFX_CORE
				).Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
		}

		private static bool CheckTargetEligibility( MemberInfo member )
		{
			var asProperty = member as PropertyInfo;
			var asField = member as FieldInfo;
			Type returnType;

			if ( asProperty != null )
			{
				if ( asProperty.GetIndexParameters().Length > 0 )
				{
					// Indexer cannot be target except the type itself implements IDictionary or IDictionary<TKey,TValue>
					return false;
				}

#if !NETFX_CORE
				if ( asProperty.GetSetMethod( true ) != null )
#else
				if ( asProperty.SetMethod != null )
#endif
				{
					return true;
				}

				returnType = asProperty.PropertyType;
			}
			else if ( asField != null )
			{
				if ( !asField.IsInitOnly )
				{
					return true;
				}

				returnType = asField.FieldType;
			}
			else
			{
				return true;
			}

			var traits = returnType.GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					return traits.AddMethod != null;
				}
				default:
				{
					return false;
				}
			}
		}

		private static void VerifyNilImplication( Type type, IEnumerable<SerializingMember> entries )
		{
			foreach ( var serializingMember in entries )
			{
				if ( serializingMember.Contract.NilImplication == NilImplication.Null )
				{
					var itemType = serializingMember.Member.GetMemberValueType();

					if ( itemType != typeof( MessagePackObject )
						&& itemType.GetIsValueType()
						&& Nullable.GetUnderlyingType( itemType ) == null )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull( serializingMember.Member.ToString(), itemType, type );
					}

					bool isReadOnly;
					FieldInfo asField;
					PropertyInfo asProperty;
					if ( ( asField = serializingMember.Member as FieldInfo ) != null )
					{
						isReadOnly = asField.IsInitOnly;
					}
					else
					{
						asProperty = serializingMember.Member as PropertyInfo;
// 20150616 applibot modify
//#if DEBUG && !UNITY_IPHONE && !UNITY_ANDROID
#if DEBUG && !UNITY_IPHONE && !UNITY_ANDROID && !UNITY
						Contract.Assert( asProperty != null, serializingMember.Member.ToString() );
#endif
						isReadOnly = asProperty.GetSetMethod() == null;
					}

					if ( isReadOnly )
					{
						throw SerializationExceptions.NewNullIsProhibited( serializingMember.Member.ToString() );
					}
				}
			}
		}

		private static void VerifyKeyUniqueness( IList<SerializingMember> result )
		{
			var duplicated = new Dictionary<string, List<MemberInfo>>();
			var existents = new Dictionary<string, SerializingMember>();
			foreach ( var member in result )
			{
				if ( member.Contract.Name == null )
				{
					continue;
				}

				try
				{
					existents.Add( member.Contract.Name, member );
				}
				catch ( ArgumentException )
				{
					List<MemberInfo> list;
					if ( duplicated.TryGetValue( member.Contract.Name, out list ) )
					{
						list.Add( member.Member );
					}
					else
					{
						duplicated.Add( member.Contract.Name, new List<MemberInfo> { existents[ member.Contract.Name ].Member, member.Member } );
					}
				}
			}

			if ( duplicated.Count > 0 )
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Some member keys specified with custom attributes are duplicated. Details: {{{0}}}",
						String.Join(
							",",
							duplicated.Select(
								kv => String.Format(
									CultureInfo.CurrentCulture,
									"\"{0}\":[{1}]",
									kv.Key,
									String.Join( ",", kv.Value.Select( m => String.Format( "{0}.{1}({2})", m.DeclaringType, m.Name, ( m is FieldInfo ) ? "Field" : "Property" ) ).ToArray() )
								)
							).ToArray()
						)
					)
				);
			}
		}
	}
}
