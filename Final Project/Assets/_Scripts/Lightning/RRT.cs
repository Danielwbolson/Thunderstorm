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

        // Keep growing our tree until it contains the goal and we've
        // grown for the required number of iterations
        while (!reachedGoal && i < maxIters) {
            // Get a random node somewhere in the direction of the goal
            Node random = RandomSample(start, goal);

            // Get the closest node in the tree to the sample
            Node closest = t.GetClosestNode(random);

            // Create a new node between the closest node and the sample
            Node extension = ExtendToward(closest, random);

            // if we have managed to create a new node, add it to the tree
            if (extension != null) {
                closest.AddChild(extension);
                t._nodeList.Add(extension);

                // If we haven't yet reached the goal, and the new node
                // is the goal, add the goal to the tree
                if (!reachedGoal && extension.GetPosition() == goal.GetPosition()) {
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
    private Node RandomSample(Node start, Node goal) {
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
    private Node ExtendToward(Node close, Node rand) {
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
