using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public float cloud_rad = 20.0f; // TODO: this is defaulted
    public float node_rad = 1.0f; // radius of each 'node'
    public int cnt = 0;
    public GameObject particleObj; // objec type instintiated for each cloud

    // Use this for initialization
    void Start() {
        // add origin
        Vector3 origin = transform.position; // set it to 'this' position
        //int cnt = (int)cloud_rad * 2;
        List<Vector3> _pos; // store local position of all nodes for blast processing
        _pos = new List<Vector3>();
        _pos.Add(origin); 
        
        // generate points
        for (int i = 0; i < cnt; i++) {
            int tries = 0;

            // try to arbitrarily place point, such that it is connected to all other positions
            while (tries < 200) { // limit tries to avoid infinte loop when debugging 
                float r = Random.Range(0.0f, cloud_rad);
                float angle = Random.Range(0.0f, 2.0f * Mathf.PI); // angle around circle

                float x = r * Mathf.Sin(angle);
                float y = r * Mathf.Cos(angle);
                float z = Random.Range(-cloud_rad, cloud_rad);
                Vector3 p = new Vector3(x, y, z);
                if (connected(p, _pos, node_rad)) {
                    _pos.Add(p);
                    GameObject o = Instantiate(particleObj, p, Quaternion.identity);
                    
                    // set transform (pos, scale)
                    o.GetComponent<Transform>().position = p;
                    o.GetComponent<Transform>().localScale = new Vector3(node_rad*2, node_rad*2, node_rad*2);
                    o.transform.parent = transform; // set the  parent of 'o' to 'this'
                    //Debug.Log("Placed point. Redos needed: " + tries);
                    break;
                }
                tries++;
                //if (tries >= 200) Debug.Log("Failed to find a good pos");
            }
        }
    }

    // checks if random generated position is 'touching' any other nodes within a radius
    bool connected(Vector3 p, List<Vector3> pos, float rad) {
        // check if in radius of any of existing sphere (assumes all are same rad)
        foreach (Vector3 iter in pos) {
            if (Vector3.Distance(p, iter) < rad * 1.5f) {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update() {

    }

}
