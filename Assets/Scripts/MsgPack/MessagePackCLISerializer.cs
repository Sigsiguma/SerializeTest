using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.IO;
using MsgPack.Serialization;

namespace msgpack {
    public class MessagePackCLISerializer : MonoBehaviour {

        private byte[] data_;
        private SerializeData serializeData_;
        private MessagePackSerializer<SerializeData> serializer_;

        private void Start() {
            serializer_ = MessagePackSerializer.Get<SerializeData>();
            serializeData_ = new SerializeData();
        }

        public void Serialize() {
            var stream = new MemoryStream();

            utility.StopWatchUtil.CountStart();
            serializer_.Pack(stream, serializeData_);
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("MessagePack CLI Serialize");

            // ストリームからデータを取り出す
            data_ = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(data_, 0, (int)stream.Length);
        }

        public void Deserialize() {
            // MessagePack Deserialize
            utility.StopWatchUtil.CountStart();
            SerializeData result = serializer_.UnpackSingleObject(data_);
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("MessagePack CLI Deserialize");
        }
    }
}