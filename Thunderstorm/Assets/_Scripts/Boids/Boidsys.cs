using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boidsys : MonoBehaviour {
  //List<GameObject> boids;
  public GameObject particleObj; // objec type instintiated for each cloud
  List<GameObject> obstacles = new List<GameObject>();
  // Use this for initialization
  void Start () {
    for (int i = 0; i < 10; i++) {
      Vector3 p = new Vector3(
            Random.Range(-2.0f,2.0f),
            3.7f, // hardcoded to this particleobj
            Random.Range(3.0f,9.0f));
      GameObject boid = Instantiate(particleObj, p, Quaternion.identity);
      boid.transform.position = p;
      boid.transform.parent = transform; // set the  parent of 'o' to 'this'
    }
  }
}
