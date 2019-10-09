using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSystem : MonoBehaviour {

    // Use this for initialization
    private List<GameObject> __obj = new List<GameObject>();
    public float width = 20.0f; // TODO: this is defaulted
    public float length = 20.0f; // TODO: this is defaulted
    public int cnt = 0;
    public GameObject particleObj; // objec type instintiated for each cloud

    void Start() {
        for (int i = 0; i < cnt; i++) {
            addCloud();
        }
    }

    void addCloud() {
        Vector3 p = new Vector3(
                Random.Range(-width / 2, width / 2),
                0,
                Random.Range(-width / 2, width / 2));
        GameObject o = Instantiate(particleObj, p, Quaternion.identity);
        o.GetComponent<Transform>().position = p + transform.position;
        o.transform.parent = transform; // set the  parent of 'o' to 'this'
                                        //Debug.Log("Placed point. Redos needed: " + tries);
        __obj.Add(o);
    }

    // Update is called once per frame
    void Update() {
        foreach (GameObject g in __obj) {
            g.GetComponent<Transform>().position += new Vector3(
                0,
                0,
                0.1f
                );
        }
        for (int i = 0; i < 5; i++) {
            addCloud();
        }
    }
}
