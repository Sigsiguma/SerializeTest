using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace msgpack {
    public class MessagePackCsharpSerializer : MonoBehaviour {

        private byte[] data_;

        public void Serialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    data_ = MessagePack.MessagePackSerializer.Serialize(SerializeDataCreator.serializeDataList_);
                }
            }, "MessagePack Csharp Serialize");
        }

        public void Deserialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    MessagePack.MessagePackSerializer.Deserialize<SerializeDataList>(data_);
                }
            }, "MessagePack Csharp Deserialize");
        }

    }
}
