using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace json {
    public class JSONSerializer : MonoBehaviour {

        private void Start() {
            SerializeData test = new SerializeData();
            string jsonData = JsonUtility.ToJson(test);
            Debug.Log(jsonData);
        }
    }
}