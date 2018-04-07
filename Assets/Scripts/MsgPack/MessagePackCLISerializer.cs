using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.IO;
using MsgPack.Serialization;

namespace msgpack {
    public class MessagePackCLISerializer : MonoBehaviour {

        private byte[] data_;
        private MessagePackSerializer<SerializeDataList> serializer_;

        private void Start() {
            serializer_ = MessagePackSerializer.Get<SerializeDataList>();
        }

        public void Serialize() {
            var stream = new MemoryStream();

            utility.StopWatchUtil.MeasureMethod(() => serializer_.Pack(stream, SerializeDataCreator.serializeDataList_), "MessagePack CLI Serialize");

            // ストリームからデータを取り出す
            data_ = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(data_, 0, (int)stream.Length);
        }

        public void Deserialize() {
            utility.StopWatchUtil.MeasureMethod(() => serializer_.UnpackSingleObject(data_), "MessagePack CLI Deserialize");
        }
    }
}