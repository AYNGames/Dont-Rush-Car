using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour {
    
    public float rot;

    void Start() {
        if(Random.Range(0,2) == 0) {
            rot = Random.Range(20 + Vars.obstacle, 40 + Vars.obstacle);
        }else {
            rot = Random.Range(-(40 + Vars.obstacle), -(20 + Vars.obstacle));
        }
    }

    void Update() {
        transform.Rotate(0, rot * Time.deltaTime, 0);
    }
}
