using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour {

    // Use this for initialization
    void Awake () {
        Random.InitState(System.DateTime.Now.Second);
    }
}
