using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

namespace protobuf {
    public class ProtoBufSerializer : MonoBehaviour {

        private ByteString data_;
        private SerializeData serializeData_;

        private void Start() {
            serializeData_ = new SerializeData();
            serializeData_.TestNum = 1;
            serializeData_.TestFloat = 3.14f;
            serializeData_.TestString = "test!!";
            serializeData_.TestBool = false;

            serializeData_.TestList.Add("enemy1");
            serializeData_.TestList.Add("enemy2");
            serializeData_.TestList.Add("enemy3");

            serializeData_.TestDic.Add(1, "test");
            serializeData_.TestDic.Add(2, "hoge");
        }

        public void Serialize() {
            utility.StopWatchUtil.CountStart();
            data_ = serializeData_.ToByteString();
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("ProtocolBuffer Serialize");
        }

        public void Deserialize() {
            utility.StopWatchUtil.CountStart();
            SerializeData.Parser.ParseFrom(data_);
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("ProtocolBuffer Deserialize");
        }
    }
}