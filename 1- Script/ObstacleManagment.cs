using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManagment : MonoBehaviour
{

    public void CreateObstacle(int index) {
        if(index == 0)
        {
            int randomObstacle = Random.Range(1, 10);

            GameObject obstacle = Instantiate(Resources.Load("Obstacle" + randomObstacle) as GameObject);
            obstacle.transform.position = new Vector3(0, 0, 50 * Menus.Static.obstacle);

            //for 2p
            GameObject obstacle2 = Instantiate(Resources.Load("Obstacle" + randomObstacle) as GameObject);
            obstacle2.transform.position = new Vector3(-25, 0, 50 * Menus.Static.obstacle);
        }
        else if(index == 1)
        {

            GameObject obstacle = Instantiate(Resources.Load("ObstacleFinish") as GameObject);         
            obstacle.transform.position = new Vector3(0, 0, 50 * Menus.Static.obstacle);

            //for 2p
            GameObject obstacle2 = Instantiate(Resources.Load("ObstacleFinish") as GameObject);
            obstacle2.transform.position = new Vector3(-25, 0, 50 * Menus.Static.obstacle);
        }       
        Menus.Static.obstacle++;
    }
}
