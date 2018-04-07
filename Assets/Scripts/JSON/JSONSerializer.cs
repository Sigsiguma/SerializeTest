using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace json {
    public class JSONSerializer : MonoBehaviour {

        private string jsonData_;
        private SerializeData serializeData_;

        private void Start() {
            serializeData_ = new SerializeData();
        }

        public void Serialize() {
            utility.StopWatchUtil.CountStart();
            jsonData_ = JsonUtility.ToJson(serializeData_);
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("JSONUtility Serialize");
        }

        public void Deserialize() {
            utility.StopWatchUtil.CountStart();
            JsonUtility.FromJson<SerializeData>(jsonData_);
            utility.StopWatchUtil.CountEnd();
            utility.StopWatchUtil.PrintTimeString("JSONUtility Deserialize");
        }
    }
}