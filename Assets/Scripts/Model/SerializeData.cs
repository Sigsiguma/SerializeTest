using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MsgPack.Serialization;

namespace model {
    [System.Serializable]
    [MessagePack.MessagePackObject]
    public class SerializeData : ISerializationCallbackReceiver {

        [MessagePackMember(0)]
        [MessagePack.Key(0)]
        public int testNum_;
        [MessagePackMember(1)]
        [MessagePack.Key(1)]
        public float testFloat_;
        [MessagePackMember(2)]
        [MessagePack.Key(2)]
        public string testString_;
        [MessagePackMember(3)]
        [MessagePack.Key(3)]
        public bool testBool_;

        [MessagePackMember(4)]
        [MessagePack.Key(4)]
        public List<string> testList_;
        [MessagePackMember(5)]
        [MessagePack.Key(5)]
        public Dictionary<int, string> testDic_;

        [HideInInspector]
        [MessagePack.IgnoreMember]
        public List<int> keys_;
        [HideInInspector]
        [MessagePack.IgnoreMember]
        public List<string> values_;

        public SerializeData() {
            testNum_ = 1;
            testFloat_ = 3.14f;
            testString_ = "test!!";
            testBool_ = false;

            testList_ = new List<string>() { "enemy1", "enemy2", "enemy3" };
            testDic_ = new Dictionary<int, string>() { { 1, "test" }, { 2, "hoge" } };

            keys_ = new List<int>();
            values_ = new List<string>();
        }

        public void OnBeforeSerialize() {
            keys_.Clear();
            values_.Clear();

            foreach (var pair in testDic_) {
                keys_.Add(pair.Key);
                values_.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize() {
            testDic_ = new Dictionary<int, string>();
            for (int i = 0; i < keys_.Count; ++i) {
                testDic_.Add(keys_[i], values_[i]);
            }
        }
    }
}