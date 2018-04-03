using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model {
    [System.Serializable]
    public class SerializeData : ISerializationCallbackReceiver {

        public int testNum_;
        public float testFloat_;
        public string testString_;
        public bool testBool_;

        public int[] testArray_;
        public List<string> testList_;
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

            testArray_ = new int[] { 2, 3, 5, 7, 11 };
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