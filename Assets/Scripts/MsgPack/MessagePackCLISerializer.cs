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

            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    data_ = serializer_.PackSingleObject(SerializeDataCreator.serializeDataList_);
                }
            }, "MessagePack CLI Serialize");

            utility.ByteSizeUtil.PrintByteSize(data_, "MessagePackCLI");
        }

        public void Deserialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    serializer_.UnpackSingleObject(data_);
                }
            }, "MessagePack CLI Deserialize");
        }
    }
}