using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Simple node class. Used inside Tree. Each node has a position and a list
 * of children
 */
public class Node {

    private Vector3 _position;
    private List<Node> _children;

    public Node() {
        _position = new Vector3();
        _children = new List<Node>();
    }

    public Node(Vector3 pos) {
        _position = pos;
        _children = new List<Node>();
    }

    public void AddChild(Node n) {
        _children.Add(n);
    }

    public List<Node> GetChildren() {
        return _children;
    }

    public Vector3 GetPosition() {
        return _position;
    }

    public void SetPosition(Vector3 pos) {
        _position = pos;
    }
}
