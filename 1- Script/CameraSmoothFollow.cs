using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public int cameraAngleIndex = 0;
    float timeCount = 0.0f;
    void OnEnable() {
        target = Menus.Static.playerGM.transform;

        chanceCameraIndex();
    }

    public void chanceCameraIndex()
    {
        cameraAngleIndex = PlayerPrefs.GetInt("debugAngleIndex", 0);
    }
    bool gameOver = false;
    void FixedUpdate() {


        if (target == null)
        {
            target = Menus.Static.playerGM.transform;
        }
        Vector3 targetPosition;
        switch (Menus.Static.gameStatusIndex)
        {

            case 0: // main menu

                targetPosition = target.TransformPoint(new Vector3(5, 1.8f, -3));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                transform.eulerAngles = new Vector3(18, -65, 0);
                GetComponent<Camera>().fieldOfView = 85;
                break;
            case 1: // shop menu
                
                targetPosition = shopPos.TransformPoint(new Vector3(0, 3, -6));
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                transform.eulerAngles = new Vector3(18, -180, 0);
                GetComponent<Camera>().fieldOfView = 85;

                break;
            case 2: // ingame

               
                switch (cameraAngleIndex)
                {
                    case 0:
                        targetPosition = target.TransformPoint(new Vector3(0, 4, -4));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        transform.eulerAngles = new Vector3(25, 0, 0);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                    case 1:
                        targetPosition = target.TransformPoint(new Vector3(2, 3, -3));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        transform.eulerAngles = new Vector3(18, -15, 0);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                }
                break;
            case 3: // tapFast


                switch (cameraAngleIndex)
                {
                    case 0:
                        targetPosition = target.TransformPoint(new Vector3(0, 5, -7));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                    case 1:
                        targetPosition = target.TransformPoint(new Vector3(2, 3, -7));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        transform.eulerAngles = new Vector3(18, -15, 0);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                }
                break;
            case 4: // xPointAre


                switch (cameraAngleIndex)
                {
                    case 0:
                        targetPosition = target.TransformPoint(new Vector3(0, 5, -5));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                    case 1:
                        targetPosition = target.TransformPoint(new Vector3(2, 3, -5));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        transform.eulerAngles = new Vector3(18, -15, 0);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                }
                break;
            case 5: // finish


                switch (cameraAngleIndex)
                {
                    case 0:
                        targetPosition = target.TransformPoint(new Vector3(0, 5, -5));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                    case 1:
                        targetPosition = target.TransformPoint(new Vector3(2, 3, -5));
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                        transform.eulerAngles = new Vector3(18, -15, 0);
                        GetComponent<Camera>().fieldOfView = 85;
                        break;
                }
                break;



        }



        //if (isShopOpen)
        //{
           
        //}
        //else
        //{

        //    if(target == null)
        //    {
        //        target = Menus.Static.playerGM.transform;
        //    }
        //    if (Menus.Static.isOver == false)
        //    {
        //        if (gameOver == false)
        //        {
        //            Vector3 targetPosition;
        //            switch (cameraAngleIndex)
        //            {
        //                case 0:
        //                    targetPosition = target.TransformPoint(new Vector3(0, 5, -3));
        //                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //                    GetComponent<Camera>().fieldOfView = 85;
        //                    break;
        //                case 1:
        //                    targetPosition = target.TransformPoint(new Vector3(2, 3, -3));
        //                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //                    transform.eulerAngles = new Vector3(18, -15, 0);
        //                    GetComponent<Camera>().fieldOfView = 85;
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            transform.position = Vector3.SmoothDamp(transform.position, gameOverTrans.position, ref velocity, smoothTime);
        //            transform.rotation = Quaternion.Lerp(transform.rotation, gameOverTrans.rotation, timeCount * smoothTime);
        //            timeCount = timeCount + Time.deltaTime;
        //        }
        //    }
        //    else
        //    {
        //        Vector3 targetPosition;
        //        switch (cameraAngleIndex)
        //        {
        //            case 0:
        //                targetPosition = target.TransformPoint(new Vector3(0, 5, -3));
        //                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //                GetComponent<Camera>().fieldOfView = 85;
        //                break;
        //            case 1:
        //                targetPosition = target.TransformPoint(new Vector3(2, 3, -5));
        //                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //                transform.eulerAngles = new Vector3(18, -15, 0);
        //                GetComponent<Camera>().fieldOfView = 85;
        //                break;
        //        }
        //    }
        //}
       
    }
    public Transform shopPos;
    bool isShopOpen = false;
    public void shopOpenClose(bool result )
    {
        isShopOpen = result;

    }
    bool doOnce = false;
    Transform gameOverTrans;
    public void gameOverAnim(Transform _gameOverTrans)
    {
        if (doOnce)
            return;

        doOnce = true;
        gameOver = true;
        gameOverTrans = _gameOverTrans;
    }
}
