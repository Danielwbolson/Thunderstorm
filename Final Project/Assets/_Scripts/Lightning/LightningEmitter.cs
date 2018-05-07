using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Class that produces lighting segments that together represent a bolt of lightning
 * 
 * Each lightning bolt is a collection of instantiated segments, where each segment is a quad, 
 * rotated to look at the camera
 */

public class LightningEmitter : MonoBehaviour {

    public float maxHeight;

    // Script references
    RRT rrt;

    // Local variables for this script
    List<Tree> rrtList;
    Camera _main_camera;
    Vector3 mc_position;

    public GameObject lightningSegment;
    public GameObject lightFlash;
    private List<GameObject> segments;

    private float time = 0.0f;
    private float period;
    private float flashTime;

    public bool userInteraction;

    // Use this for initialization
    void Start() {
        rrt = GetComponent<RRT>();
        segments = new List<GameObject>();
        rrtList = new List<Tree>();

        _main_camera = Camera.main;
        mc_position = _main_camera.transform.position;

        period = 5.0f;
        flashTime = 0.5f;
        userInteraction = false;
    }

    // Update is called once per frame
    void Update() {
        if (!userInteraction) {
            time += Time.deltaTime;

            if (time >= period) {
                time -= period;
                period = Random.Range(0.0f, 10.0f);
                LightningRealTime();
            }
        }
    }

    /*
     * @brief Creates an RRT and draws lightning in realtime
     */
    public void LightningRealTime(Node nGoal = null) {
        Vector3 goal;
        Vector3 start;

        if (nGoal == null) {
            goal = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(3.5f, 5f), 6);
            nGoal = new Node(goal);
            start = new Vector3(Random.Range(-3.0f, 3.0f), maxHeight, goal.z);
        } else {
            goal = nGoal.GetPosition();
            start = new Vector3(goal.x + Random.Range(-1.0f, 1.0f), maxHeight, goal.z);
        }

        Node nStart = new Node(start);

        int iterations = 500;

        Tree t = rrt.Path(nStart, nGoal, iterations);

        DrawLightning(t);
    }

    /*
     * @brief Function that draws and rotates each lightning segment
     */
    private void DrawLightning(Tree t = null) {
        // Get a random lightning composition
        if (t == null) {
            int i = Random.Range(0, 499);
            t = rrtList[i];
        }
        // Scale for lightning at top of tree
        float scaleX = 0.05f;

        // Create a queue for our nodes and go through BFS style
        Queue<Node> myQueue = new Queue<Node>();
        myQueue.Enqueue(t._root);

        while (myQueue.Count != 0) {
            // Dequeue our current node and cache it's position
            Node curr = myQueue.Dequeue();
            Vector3 currPos = curr.GetPosition();

            // decrease our scale to give lightning a more realistic look
            scaleX *= 0.996f;

            foreach (Node c in curr.GetChildren()) {
                // cache our positons for the child node
                Vector3 childPos = c.GetPosition();

                // enqueue our child nodes
                myQueue.Enqueue(c);

                // get the vector between the two nodes
                Vector3 dir = childPos - currPos;

                // instantiate the lightning segment directly inbetween the two nodes
                Vector3 ls_position = 0.5f * (currPos + childPos);
                GameObject ls = Instantiate(lightningSegment, ls_position, Quaternion.identity) as GameObject;

                // align up of lightning with the vector between nodes
                ls.transform.up = dir;

                // rotate to face the camera
                float angle = Vector3.Angle(Vector3.forward, new Vector3(
                    mc_position.x - ls_position.x,
                    0,
                    mc_position.z - ls_position.z));
                ls.transform.RotateAround(ls_position, ls.transform.up, angle);

                // scale the y length of the lightning segment to fit between the points
                float scaleY = 1.1f * Mathf.Abs(Vector3.Distance(curr.GetPosition(), c.GetPosition()));
                ls.transform.localScale = new Vector3(scaleX, scaleY, 1);

                // add to our list so we can delete later
                segments.Add(ls);
            }
        }
        // Now that we have our lightning, flash the light at it's root
        CreateLightningFlash(t._root.GetPosition());
    }

    /*
     * @brief Instantiates a an instance of LightningFlash at the given position
     */
    void CreateLightningFlash(Vector3 pos) {
        GameObject lf = Instantiate(lightFlash) as GameObject;
        lf.transform.position = new Vector3(pos.x, pos.y + 3, 7);
        lf.GetComponent<LightningFlash>().flashLength = flashTime + 0.1f;
    }

    public void UserInteraction() {
        userInteraction = !userInteraction;
    }
}
