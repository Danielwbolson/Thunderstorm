using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UserInteraction : MonoBehaviour {

    private GameObject le;
    bool active;
    Button b;

    private void Start() {
        active = false;
        le = GameObject.FindGameObjectWithTag("Emitter");

        b = GetComponentInChildren<Button>();
        b.GetComponentInChildren<Text>().text = "Inactive";
    }

    public void OnClick() {
        le.GetComponent<LightningEmitter>().UserInteraction();
        active = !active;

        if (active) {
            b.GetComponentInChildren<Text>().text = "Active";
        } else {
            b.GetComponentInChildren<Text>().text = "Inactive";
        }
    }
}
