#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(6)
            {
                {typeof(global::System.Collections.Generic.List<string>), 0 },
                {typeof(global::System.Collections.Generic.Dictionary<int, string>), 1 },
                {typeof(global::System.Collections.Generic.List<global::model.SerializeData>), 2 },
                {typeof(global::MessagePack.MessagePackType), 3 },
                {typeof(global::model.SerializeData), 4 },
                {typeof(global::model.SerializeDataList), 5 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<string>();
                case 1: return new global::MessagePack.Formatters.DictionaryFormatter<int, string>();
                case 2: return new global::MessagePack.Formatters.ListFormatter<global::model.SerializeData>();
                case 3: return new MessagePack.Formatters.MessagePack.MessagePackTypeFormatter();
                case 4: return new MessagePack.Formatters.model.SerializeDataFormatter();
                case 5: return new MessagePack.Formatters.model.SerializeDataListFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.MessagePack
{
    using System;
    using MessagePack;

    public sealed class MessagePackTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.MessagePackType>
    {
        public int Serialize(ref byte[] bytes, int offset, global::MessagePack.MessagePackType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, (Byte)value);
        }
        
        public global::MessagePack.MessagePackType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            return (global::MessagePack.MessagePackType)MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }


}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.model
{
    using System;
    using MessagePack;


    public sealed class SerializeDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::model.SerializeData>
    {

        public int Serialize(ref byte[] bytes, int offset, global::model.SerializeData value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 6);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.testNum_);
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.testFloat_);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.testString_, formatterResolver);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.testBool_);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref bytes, offset, value.testList_, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, string>>().Serialize(ref bytes, offset, value.testDic_, formatterResolver);
            return offset - startOffset;
        }

        public global::model.SerializeData Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __testNum___ = default(int);
            var __testFloat___ = default(float);
            var __testString___ = default(string);
            var __testBool___ = default(bool);
            var __testList___ = default(global::System.Collections.Generic.List<string>);
            var __testDic___ = default(global::System.Collections.Generic.Dictionary<int, string>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __testNum___ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __testFloat___ = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                        break;
                    case 2:
                        __testString___ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __testBool___ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 4:
                        __testList___ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __testDic___ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, string>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::model.SerializeData();
            ____result.testNum_ = __testNum___;
            ____result.testFloat_ = __testFloat___;
            ____result.testString_ = __testString___;
            ____result.testBool_ = __testBool___;
            ____result.testList_ = __testList___;
            ____result.testDic_ = __testDic___;
            return ____result;
        }
    }


    public sealed class SerializeDataListFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::model.SerializeDataList>
    {

        public int Serialize(ref byte[] bytes, int offset, global::model.SerializeDataList value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::model.SerializeData>>().Serialize(ref bytes, offset, value.dataList_, formatterResolver);
            return offset - startOffset;
        }

        public global::model.SerializeDataList Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __dataList___ = default(global::System.Collections.Generic.List<global::model.SerializeData>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __dataList___ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::model.SerializeData>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::model.SerializeDataList();
            ____result.dataList_ = __dataList___;
            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
