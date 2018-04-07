using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MsgPack.Serialization;

namespace model {
    [System.Serializable]
    public class SerializeData : ISerializationCallbackReceiver {

        [MessagePackMember(1)]
        public int testNum_;
        [MessagePackMember(2)]
        public float testFloat_;
        [MessagePackMember(3)]
        public string testString_;
        [MessagePackMember(4)]
        public bool testBool_;

        [MessagePackMember(5)]
        public List<string> testList_;
        [MessagePackMember(6)]
        public Dictionary<int, string> testDic_;

        [HideInInspector]
        public List<int> keys_;
        [HideInInspector]
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