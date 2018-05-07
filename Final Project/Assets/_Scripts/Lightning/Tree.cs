using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Class that represents a tree. Keeps a list of nodes and its root
 */
public class Tree {

    [HideInInspector]
    public Node _root;
    [HideInInspector]
    public List<Node> _nodeList;

    public Tree() {
        _nodeList = new List<Node>();
        _root = new Node();
    }

    /*
     * @brief Returns the closest node in the tree to the inputted node
     */
    public Node GetClosestNode(Node rand) {
        float dist = Mathf.Infinity;
        Node closest = new Node();
        for (int i = 0; i < _nodeList.Count; i++) {
            float distanceBetween = DistanceBetween(_nodeList[i], rand);
            if (distanceBetween < dist) {
                dist = distanceBetween;
                closest = _nodeList[i];
            }
        }
        return closest;
    }

    /*
     * @brief Returns the distance between two nodes
     */
    private float DistanceBetween(Node a, Node b) {
        Vector3 apos = a.GetPosition();
        Vector3 bpos = b.GetPosition();

        return Vector3.Distance(apos, bpos);
    }
}
