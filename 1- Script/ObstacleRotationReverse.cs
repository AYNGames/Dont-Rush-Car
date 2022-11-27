using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotationReverse : MonoBehaviour
{
    public ObstacleRotation obstacleRotation;
    
    void Update() {
        transform.Rotate(0, -obstacleRotation.rot * Time.deltaTime, 0);
    }
}
