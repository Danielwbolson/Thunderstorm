using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSegment : MonoBehaviour {

    float uptime = 0.5f;
    float time = 0.0f;
    
    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;

        if (time > uptime) {
            Destroy(gameObject);
        }
    }
}
