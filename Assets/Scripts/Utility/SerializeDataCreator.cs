using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

public class SerializeDataCreator : MonoBehaviour {

    public static int listLength_ { get; } = 1000;
    public static SerializeDataList serializeDataList_ { get; private set; }

    private void Awake() {
        serializeDataList_ = new SerializeDataList(listLength_);
    }

}