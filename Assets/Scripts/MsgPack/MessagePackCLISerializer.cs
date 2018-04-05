using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.IO;
using MsgPack.Serialization;

namespace msgpack {
    public class MessagePackCLISerializer : MonoBehaviour {

        private void Start() {
            SerializeData test = new SerializeData();
            // MessagePack Serialize
            var stream = new MemoryStream();
            var serializer = MessagePackSerializer.Get<SerializeData>();
            serializer.Pack(stream, test);

            // ストリームからデータを取り出す
            byte[] data = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(data, 0, (int)stream.Length);

            Debug.Log(MessagePack.MessagePackSerializer.ToJson(data));

            // MessagePack Deserialize
            SerializeData result = serializer.UnpackSingleObject(data);

        }
    }
}