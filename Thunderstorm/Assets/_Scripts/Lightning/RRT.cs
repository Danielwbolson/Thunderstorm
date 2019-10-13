using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /*
  * @brief Class that represents a biased RRT.
  * 
  * Instead of randomly sampling everywhere, the RRT samples downwards in a cone shape
  */
public class RRT : MonoBehaviour {

    Tree t;
    public float maxHeight;
    private int iter = 1;

    /*
     * @brief Returns a tree that uses an RRT to pathfind from a root to a goal
     */
    public Tree Path(Node start, Node goal, int maxIters) {
        t = new Tree();
        t._root = start;
        t._nodeList.Add(start);

        bool reachedGoal = false;
        int i = 0;

        while (!reachedGoal && i < maxIters) {
            Node random = RandomNode(start, goal);

            Node closest = t.GetClosestNode(random);

            Node newNode = CalculateNewNode(closest, random);

            if (newNode != null) {
                closest.AddChild(newNode);
                t._nodeList.Add(newNode);

                if (!reachedGoal && newNode.GetPosition() == goal.GetPosition()) {
                    reachedGoal = true;
                }
            }
            i++;
            iter++;
        }
        return t;
    }

    /*
     * @brief Provides a Random Node that is either the goal or a random node
     * within a cone extending from the start node to the ground
     */
    private Node RandomNode(Node start, Node goal) {
        Vector3 spos = start.GetPosition();
        Vector3 gpos = goal.GetPosition();

        float tan_theta = Random.Range(0.2f, 0.5f); // tangent of our desired angle

        if (iter%30 == 0) {
            iter = 0;
            return goal;
        } else {
            float newY = Random.Range(gpos.y, maxHeight);
            float newX = Random.Range(-(maxHeight - newY) * tan_theta + spos.x, (maxHeight - newY) * tan_theta + spos.x);

            Vector3 newPos = new Vector3(newX, newY, gpos.z);
            Node newNode = new Node(newPos);
            return newNode;
        }

    }

    /*
     * @brief Takes our random node and extends in the direction of it 
     * a certain distance. Creates a node in the direction of the random node
     * instead of using the random node
     */
    private Node CalculateNewNode(Node close, Node rand) {
        Vector3 closePos = close.GetPosition();
        Vector3 randPos = rand.GetPosition();

        float maxDistance = 0.05f;
        if (Vector3.Distance(closePos, randPos) < maxDistance) {
            return rand;
        } else {
            Vector3 newPos = closePos + maxDistance * Vector3.Normalize(randPos - closePos);
            Node newNode = new Node(newPos);
            return newNode;
        }
    }
}
