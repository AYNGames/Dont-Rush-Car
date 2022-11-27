using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class buildingCtrl : MonoBehaviour
{


    public GameObject[] buldings;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, buldings.Length);
        for (int i = 0; i < buldings.Length; i++)
        {
            if(i == rand)
            {

                buldings[i].SetActive(true);
              //  buldings[i].transform.position = new Vector3(buldings[i].transform.position.x, buldings[i].transform.position.y + Random.Range(-5, 5), buldings[i].transform.position.x);
            }
            else
            {
                buldings[i].SetActive(false);
            }
        }
    }

    

}
