using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

namespace protobuf {
    public class ProtoBufSerializer : MonoBehaviour {
        void Start() {
            var test = new SerializeData();
            test.TestNum = 1;
            test.TestFloat = 3.14f;
            test.TestString = "test!!";
            test.TestBool = false;

            test.TestList.Add("enemy1");
            test.TestList.Add("enemy2");
            test.TestList.Add("enemy3");

            test.TestDic.Add(1, "test");
            test.TestDic.Add(2, "hoge");

            // ProtocolBuffer Serialize
            ByteString bytes = test.ToByteString();

            // ProtocolBuffer Deserialize
            var result = SerializeData.Parser.ParseFrom(bytes);
        }
    }
}