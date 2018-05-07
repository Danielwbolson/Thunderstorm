using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Class that produces a flash of light at the source of a lightning bolt
 * 
 * Each flash has an intensity and flash length
 */
public class LightningFlash : MonoBehaviour {

    public float flashLength;
    public float maxIntensity;
    float time = 0;

    Light light;

    // Use this for initialization
    void Start () {
        light = GetComponent<Light>();
        light.intensity = maxIntensity;
    }
    
    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        float timeLeft = flashLength - time;

        if (timeLeft <= 0) {
            Destroy(gameObject);
        }

        float intensity = timeLeft * maxIntensity;
        light.intensity = intensity;
    }
}
