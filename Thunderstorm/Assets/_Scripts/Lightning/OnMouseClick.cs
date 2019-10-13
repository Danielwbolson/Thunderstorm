using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseClick : MonoBehaviour {

    private GameObject emitter;
    private LightningEmitter le;

    private void Start() {
        emitter = GameObject.FindGameObjectWithTag("Emitter");
        le = emitter.GetComponent<LightningEmitter>();
    }

    private void OnMouseDown() {
        if (le.userInteraction) {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
                Vector3 pos = ray.origin + (ray.direction * hit.distance);
                pos.z = 6;

                Node nGoal = new Node(pos);

                le.LightningRealTime(nGoal);
            }
        }
    }
}
