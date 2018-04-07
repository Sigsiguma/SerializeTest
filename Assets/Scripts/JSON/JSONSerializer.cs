using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace json {
    public class JSONSerializer : MonoBehaviour {

        private string jsonData_;

        public void Serialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    jsonData_ = JsonUtility.ToJson(SerializeDataCreator.serializeDataList_);
                }
            }, "JSONUtility Serialize");
        }

        public void Deserialize() {
            utility.StopWatchUtil.MeasureMethod(() => {
                for (int i = 0; i < SerializeDataCreator.iterationNum; ++i) {
                    JsonUtility.FromJson<SerializeDataList>(jsonData_);
                }
            }, "JSONUtility Deserialize");
        }
    }
}