using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
  List<GameObject> obstacles = new List<GameObject>();
  static List<Boid> friendlies = new List<Boid>(); // assume one stack of boids for now
  public Vector3 pos;
  public Vector3 vel;
  private List<Boid> _neighbors;
  public static float _friend_range = 2.5f;
  static float max_velocity = 1.0f;
  public Vector3 origin; // this doesn't change

  // Use this for initialization
  void Start() {
    vel = new Vector3(0,0,0);
    pos = new Vector3(0,0,0);
    _neighbors = new List<Boid>();
    friendlies.Add(this);
    pos = transform.position;
    origin = transform.position;
  }

  // Update is called once per frame
  void Update() {
    obstacles = new List<GameObject>(GameObject.FindGameObjectsWithTag("obstacle"));
    getFriends();

    // forces influence by a scalar
    Vector3 align = getAlignForce() * 0.5f;
    Vector3 seper = getSeperationForce() * 1.0f;
    Vector3 cohes = getCohesionForce() * 1.8f;
    Vector3 screenColl = getCollisionForce() * 1.5f; // avoid the 'screen' (2d plane)
    Vector3 avoid = getAvoidForce();

    // Generate random noise
    float nf = 0.05f; // noise factor (scalar)
    Vector3 noise = new Vector3(Random.Range(-nf, nf), Random.Range(-nf, nf), Random.Range(-nf, nf));

    // update vel
    vel += align + seper + cohes + noise + screenColl + avoid;
    vel.y = 0;
    vel = Vector3.ClampMagnitude(vel, 1.0f);
    
    // update pos
    pos += vel * 0.05f; // scale it down

    // debug: screen wrap around plane (comment out this block to disable)
    //int BOUND = 10;
    //    int XBOUND = 6;
    //    int ZBOUND = 6;
    //if (pos.x > XBOUND) pos.x = -XBOUND;
    //else if (pos.x < -XBOUND) pos.x = XBOUND;
    //if (pos.z > 6+ZBOUND) pos.z = 6-ZBOUND;
    //else if (pos.z < 6-ZBOUND) pos.z = 6+ZBOUND;

    // update actual transform
    this.transform.position = pos; // update actual pos
    this.transform.LookAt(pos + vel); // point in moving dir
  }

  // get nearby neighbors
  void getFriends() {
    _neighbors.Clear();
    foreach (Boid b in friendlies) {
      if (b == this) continue;
      if (Vector3.Distance(b.pos, this.pos) < _friend_range) {
        _neighbors.Add(b);
      }
    }
  }

  // forces boids to point same dir as other friends
  Vector3 getAlignForce() {
    Vector3 result = new Vector3(0,0,0);
    foreach (Boid friend in _neighbors) {
      float d = Vector3.Distance(this.pos, friend.pos);
      if (d > 0 && d < _friend_range) { // distance check helps for divide by zero and screen wrapping
        result += friend.vel / d;
      }
    }
    return result;
  }

  // forces boids to try and go awy from each other
  Vector3 getSeperationForce() {
    Vector3 result = new Vector3(0,0,0);
    foreach (Boid friend in _neighbors) {
      float d = Vector3.Distance(this.pos, friend.pos);
      if (d > 0 && d < _friend_range) { // distance check helps for divide by zero and screen wrapping
        Vector3 toMe = Vector3.Normalize(this.pos - friend.pos) / d;
        result += toMe;
      }
    }
    return result;
  }

  // exact same as sepeartion force except tracked by obstacles
  Vector3 getAvoidForce() {
    Vector3 result = new Vector3(0, 0, 0);
    foreach (GameObject scaryThing in obstacles) {
      Vector3 scaryPos = scaryThing.transform.position;
      scaryPos.y = 3.6f; // makes the boids assume the obstacle is at ground level
      float d = Vector3.Distance(this.pos, scaryPos);
      if (d > 0 && d < _friend_range) { // distance check helps for divide by zero and screen wrapping
        Vector3 toMe = Vector3.Normalize(this.pos - scaryPos) / d;
        result += toMe;
      }
    }
    return result;
  }

  // make boids try and stick together
  Vector3 getCohesionForce() {
    Vector3 avg = new Vector3(0,0 ,0);
    int count = 0;
    foreach (Boid friend in _neighbors) {
      float d = Vector3.Distance(this.pos, friend.pos);
      if (d > 0 && d < _friend_range+1) { // distance check helps for divide by zero and screen wrapping
        avg += (friend.pos);
        count++;
      }
    }
    if (count > 0) {
      avg /= count;
      return (avg - pos);
    }
    return new Vector3(0,0,0);
  }

  // avoid the 'screen' boundaries as if they were obstacles
  Vector3 getCollisionForce() {
    Vector3 result = new Vector3(0, 0, 0);
    float checkDist = max_velocity * 1.1f; // slightly larger than the max vel
    // avoidance forces are really strong, so the objects generally never reach up to the bounds
    // I fix this by padding the bounds by the check distnace, (e.g. the cows will stay within bounds of 6 and 4, respectivly)
    float XBOUND = 3.0f + checkDist; // bound is 6 units away from the origin (e.g. 12 units of width)
    float ZBOUND = 3.0f + checkDist; // bound is 4 units away from the origin (e.g. 8 units of width)
    // check right wall
    float d = Mathf.Abs((XBOUND + origin.x) - pos.x); 
    if (d < checkDist) result.x = -1.0f / d; // right wall

    d = Mathf.Abs((-XBOUND + origin.x) - pos.x); 
    if (d < checkDist) result.x = 1.0f / d; // left wall

    d = Mathf.Abs((ZBOUND + origin.z) - pos.z);
    if (d < checkDist) result.z = -1.0f / d; // back wall

    d = Mathf.Abs((-ZBOUND + origin.z) - pos.z);
    if (d < checkDist) result.z = 1.0f / d; // front wall

    return result;
  }
}
