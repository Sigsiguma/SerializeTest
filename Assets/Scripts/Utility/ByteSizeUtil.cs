using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility {
    public static class ByteSizeUtil {

        public static void PrintByteSize(string text, string name) {
            MessageScroll.Log(GetByteSizeString(text, name));
        }

        public static void PrintByteSize(byte[] bytes, string name) {
            MessageScroll.Log(GetByteSizeString(bytes, name));
        }

        public static string GetByteSizeString(string text, string name) {
            int byteCount = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(text);
            return string.Format("{0}: {1} bytes", name, byteCount);
        }

        public static string GetByteSizeString(byte[] bytes, string name) {
            return string.Format("{0}: {1} bytes", name, bytes.Length);
        }
    }
}
