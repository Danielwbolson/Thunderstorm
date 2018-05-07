using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReverseFaces : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] tris = new int[mesh.triangles.Length];
        tris = mesh.triangles;
        mesh.SetTriangles(tris.Reverse().ToArray(), 0);
    }
}
