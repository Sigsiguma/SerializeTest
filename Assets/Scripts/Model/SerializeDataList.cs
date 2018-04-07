using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MsgPack.Serialization;

namespace model {
    [System.Serializable]
    [MessagePack.MessagePackObject]
    public class SerializeDataList {

        [MessagePackMember(0)]
        [MessagePack.Key(0)]
        public List<SerializeData> dataList_;

        public void Init(int listSize) {
            dataList_ = new List<SerializeData>();
            for (int i = 0; i < listSize; ++i) {
                dataList_.Add(new SerializeData());
            }
        }
    }
}
