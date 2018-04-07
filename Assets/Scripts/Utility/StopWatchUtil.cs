using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace utility {
    public static class StopWatchUtil {

        private static Stopwatch sw_ = new Stopwatch();

        public static void CountStart() {
            sw_.Restart();
        }

        public static void CountEnd() {
            sw_.Stop();
        }

        public static string GetTimeString(string timeName) {
            return string.Format("{0}: {1} ms", timeName, sw_.ElapsedMilliseconds);
        }

        public static void PrintTimeString(string timeName) {
            MessageScroll.Log(GetTimeString(timeName));
        }

    }
}