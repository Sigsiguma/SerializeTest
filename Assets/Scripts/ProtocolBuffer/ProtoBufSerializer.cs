using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

namespace protobuf {
    public class ProtoBufSerializer : MonoBehaviour {

        private ByteString data_;
        private SerializeDataList serializeDataList_;

        private void Start() {
            serializeDataList_ = new SerializeDataList();
            for (int i = 0; i < SerializeDataCreator.listLength_; ++i) {
                SerializeData serializeData = new SerializeData();
                serializeData.TestNum = 1;
                serializeData.TestFloat = 3.14f;
                serializeData.TestString = "test!!";
                serializeData.TestBool = false;

                serializeData.TestList.Add("enemy1");
                serializeData.TestList.Add("enemy2");
                serializeData.TestList.Add("enemy3");

                serializeData.TestDic.Add(1, "test");
                serializeData.TestDic.Add(2, "hoge");
                serializeDataList_.DataList.Add(serializeData);
            }
        }

        public void Serialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    data_ = serializeDataList_.ToByteString();
                }
            }, "ProtocolBuffer Serialize");
        }

        public void Deserialize() {
            SerializeDataList result = new SerializeDataList();
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    result = SerializeDataList.Parser.ParseFrom(data_);
                }
            }, "ProtocolBuffer Deserialize");
        }
    }
}