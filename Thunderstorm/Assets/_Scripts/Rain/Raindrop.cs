using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Class that represents a raindrop. Each raindrop has a position, velocity
 * and wind impacting it. 
 * 
 * Raindrops fall dowards without acceleration, as they are at terminal
 * velocity. Each update we check if they are below the camera, have collided, and 
 * update their position and then their rotation to face the camera
 */
public class Raindrop : MonoBehaviour {

    const float Pi = Mathf.PI;

    Vector3 _position;
    Vector3 _velocity;
    Vector3 _wind;

    Camera _main_camera;

    void Start() {
        _position = transform.position;
        _velocity = new Vector3(0, -10f, -2f);

        _wind = new Vector3(1f, 0, 0);

        _main_camera = Camera.main;
        _velocity += _wind;
    }

    // Update is called once per frame
    void Update() {
        CheckDeath();
        UpdatePhysics(Time.deltaTime);
        UpdateRotation();
    }

    /*
     * @brief Checks if the raindrop isbelow the view of the camera. If so it is destroyed
     */
    void CheckDeath() {
        Vector3 viewport_pos = _main_camera.WorldToViewportPoint(transform.position);
        if (viewport_pos.y < 0) {
            Destroy(gameObject);
        }
    }

    /*
     * @brief Updates the position of the raindrop using euler integration
     */
    void UpdatePhysics(float dt) {
        // Euler Integration, mass of droplets is 1
        _position += _velocity * dt;
        transform.position = _position;
    }

    /*
     * @brief Updates the rotation of the raindrop to face in the direction of the camera
     */
    void UpdateRotation() {
        transform.LookAt(_main_camera.transform.position);

        float angle = Vector3.Angle(Vector3.right, _velocity);
        transform.RotateAround(transform.position, Vector3.forward, 90 - angle);
    }
}