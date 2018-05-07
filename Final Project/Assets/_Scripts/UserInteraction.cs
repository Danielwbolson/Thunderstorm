using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UserInteraction : MonoBehaviour {

    private GameObject le;
    bool active;
    Button[] b;

    private void Start() {
        active = false;
        le = GameObject.FindGameObjectWithTag("Emitter");

        b = GetComponentsInChildren<Button>();
        b[1].GetComponentInChildren<Text>().text = "Inactive";
    }

    public void OnClickActive() {
        le.GetComponent<LightningEmitter>().UserInteraction();
        active = !active;

        if (active) {
            b[1].GetComponentInChildren<Text>().text = "Active";
        } else {
            b[1].GetComponentInChildren<Text>().text = "Inactive";
        }
    }

    public void ExitSimulation() {
        Application.Quit();
    }
}
