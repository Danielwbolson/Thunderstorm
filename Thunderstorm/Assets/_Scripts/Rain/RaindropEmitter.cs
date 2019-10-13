using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Class that instantiates raindrops within a boxed area
 */
public class RaindropEmitter : MonoBehaviour {

    public GameObject raindrop;
    public int drops_per_frame;
    public int width, depth;

    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {
        CreateRaindrops();
    }

    /*
     * @brief Every frame we create a certain number of raindrops and set
     * their positions to be random positions at the top of our bounding box
     */
    void CreateRaindrops() {
        for (int i = 0; i < drops_per_frame; i++) {
            Vector3 new_transform = new Vector3(
                2 * width * Random.value - width + transform.position.x,
                transform.position.y, 
                -2 * depth * Random.value + transform.position.z);

            // create raindrop and move to position
            GameObject r = Instantiate(raindrop, new_transform, Quaternion.identity);
            r.transform.parent = transform;
        }
    }
}
