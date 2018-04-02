using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace utility {
    [RequireComponent(typeof(ScrollRect))]
    public class MessageScroll : MonoBehaviour {

        private static string logs = "";
        private string oldLogs = "";

        private ScrollRect scrollRect;
        private Text textLog;

        private void Start() {
            scrollRect = GetComponent<ScrollRect>();
            textLog = scrollRect.content.GetComponent<Text>();
        }

        private void Update() {
            if (logs != oldLogs) {
                textLog.text = logs;
                scrollRect.verticalNormalizedPosition = 0;
                oldLogs = logs;
            }
        }

        public static void Log(string logText) {
            logs += (logText + "\n");
            Debug.Log(logText);
        }

    }
}