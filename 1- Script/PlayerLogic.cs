using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{

    //Player and AI control happens here


    [Header("GENERAL")]
    public GameObject playerNameCanvas;
    public Text playerNameText;
    public float initialSpeed = 0.2f;
    private float speed = 0.2f;
    public Rigidbody rb;
    public GameObject explosion;
    public CameraSmoothFollow cameraFollow;
    public GameObject gameOverMenu;
    public GameObject confetties;
    public Transform gameOverCameraTrans;
    public Animator playerAnimator;

    [Header("AI ELEMENTS")]
    public bool isAi = false;
    float m_MaxDistance;
    float m_Speed;
    bool m_HitDetect;
    Collider m_Collider;
    RaycastHit m_Hit;
    Transform mainCamera;

 
    private void Awake()
    {
        setCharacterMode();
    }
    void Start()
    {
     
        speed = initialSpeed;
        cameraFollow = Menus.Static.cameraFollow;
        cameraFollow.enabled = true;
 

        m_MaxDistance = 32;
        m_Speed = 20.0f;
        m_Collider = GetComponent<Collider>();


        mainCamera = Camera.main.transform;
        if (isAi)
        {
            playerNameCanvas.SetActive(true);
        }
        else
        {
            playerNameCanvas.SetActive(false);
        }
    }
    void FixedUpdate()
    {

        playerNameCanvas.transform.LookAt(mainCamera);
       
        switch (Menus.Static.gameStatusIndex)
        {
            case 0:// main menu
                speed = 0;
                break;
            case 1: // shop menu


                break;
            case 2: // in game play
                if (reSpwanTime)
                    return;


                if (!isAi)
                {
                    if (Menus.Static.speedIndex == 1)
                    {
                        if (speed < 0.7f)
                        {
                            speed += 0.05f;
                        }
                    }
                    else if (Menus.Static.speedIndex == 2)
                    {
                        if (speed > 0.048F)
                        {
                            speed -= 0.02f;
                        }
                    }
                    else
                    {
                        speed = initialSpeed;
                    }
                }
                else
                {
                    calculateSpeed();

                    if (AIspeedIndex == 1)
                    {
                        if (speed < 0.7f)
                        {
                            speed += 0.05f;
                        }
                    }
                    else if (AIspeedIndex == 2)
                    {
                        if (speed > 0.048F)
                        {
                            speed -= 0.02f;
                        }
                    }
                    else
                    {
                        speed = initialSpeed;                     
                    }
                }
                if (playerAnimator)
                {
                    float _speed = 0;
                    if (speed < 0.4f)
                    {
                        _speed = 0.6f;
                    }
                    else
                    {
                        _speed = speed * 1.2f;
                    }
                    
                    playerAnimator.SetFloat("speed", _speed);
                }
                break;

            case 3: // tap fast time

                speed = 0;

                break;

            case 4: // xPointArea
                if (isAi == false)
                {
                    speed = 1;
                }
                else
                {
                    speed = 0;
                }
                break;
            case 5: // finish
                if (isAi == false)
                {
                    speed = 0;
                }
                break;

        }       
        if (isCollideObs)
            speed = 0;


        rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + speed);
    }
    int AIspeedIndex = 0;
    float maxDistance = 10;
    void calculateSpeed() // calculate speed for AI
    {

        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        if (m_HitDetect)
        {
            AIspeedIndex = 2;
        }
        else
        {
            AIspeedIndex = 1;
        }   
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }
    bool youWin = false;
    Transform respawnPos;
    bool isXpartReach = false;
    IEnumerator xPartOver()
    {
        if (currentCharacter.GetComponent<Animator>())
            currentCharacter.GetComponent<Animator>().Play("FD");

        yield return new WaitForSeconds(2.3f);
        mainSoundCtrl.Static.chanceSoundVolume(0.2f);
        Menus.Static.winSound.Play();
        cameraFollow.enabled = false;
        Menus.Static.ShowGameOverMenu(youWin, _revenueXPoint);      
    }
    float _revenueXPoint;
    void FaieldGameOver()
    {
        mainSoundCtrl.Static.chanceSoundVolume(0.2f);
        Menus.Static.loseSound.Play();
        Menus.Static.ShowGameOverMenu(false, 0);

    }
    void OnTriggerEnter(Collider col)
    {

        if (isAi == false)
        {
            if(Menus.Static.gameStatusIndex == 4)
            {
                mainSoundCtrl.Static.chanceSoundVolume(0.2f);
                Menus.Static.xpOintSoundPlay();
            }
            if (Menus.Static.tapFastCounter > 31)
            {
                if (col.transform.name.Equals("chest"))
                {

                    Menus.Static.gameStatusIndex = 5;
                    isXpartReach = true;
                    _revenueXPoint = 32f;
                    confetties.transform.SetParent(null);
                    confetties.SetActive(true);
                    StartCoroutine(xPartOver());
                }
            }
            else
            {
                if (col.transform.name.Equals(Menus.Static.tapFastCounter.ToString()))
                {                   
                    Menus.Static.gameStatusIndex = 5;
                    isXpartReach = true;
                    _revenueXPoint = float.Parse(col.transform.name.ToString());
                    confetties.transform.SetParent(null);
                    confetties.SetActive(true);
                    StartCoroutine(xPartOver());
                }
            }
        }
        if (Menus.Static.gameStatusIndex > 2)
            return;
        if (col.tag == "RS")
        {
            respawnPos = col.transform;
        }
        if (col.tag == "Finish")
        {
            Menus.Static.gameStatusIndex = 3;
            if (isAi == false)
            {
                youWin = true;
                gameOverFonk();
            }
            else
            {
                youWin = false;
                FaieldGameOver();
            }
        }     
    }
    bool isCollideObs = false;

    void CloseGamePlayUI()
    {
        Menus.Static.playerControlMenu.SetActive(false);
    }
    bool reSpwanTime = false;
    void OnCollisionEnter(Collision col)
    {      
        if (col.transform.gameObject.layer == 9)
        {
            if (isAi)
            {
                Menus.Static.hitSoundPlay(0.3f);
            }
            else
            {
                Menus.Static.hitSoundPlay(1f);
            }
            reSpwanTime = true;
            StartCoroutine(reSpawn());
            int _characterMode = PlayerPrefs.GetInt("debugCharacterIndex", 1);
            if (_characterMode == 0)
            {
                currentCharacter.GetComponent<Animator>().Play("DIE");
            }    
        }
    }
    IEnumerator reSpawn()
    {
        yield return new WaitForSeconds(1);
        transform.position = respawnPos.position;
        reSpwanTime = false;
        int _characterMode = PlayerPrefs.GetInt("debugCharacterIndex", 1);
        if (_characterMode == 0)
        {
            currentCharacter.GetComponent<Animator>().Play("RUN");
        }
    }
    void gameOverCameraAnimation()
    {
        //playerAnimator.transform.GetComponent<Invector.vCharacterController.vRagdoll>().ActivateRagdoll();
        FindObjectOfType<CameraSmoothFollow>().gameOverAnim(gameOverCameraTrans);
        Invoke("gameOverFonk", 2.5F);
    }
    public void saveMe()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 12);
    }
    public void gameOverFonk()
    {
        Menus.Static.openCloseTapFastCanvas(true);
    }
    [Header("CHARACTERS")]
    public GameObject[] characterContainer;
    public GameObject[] stickmanContainer;
    public GameObject[] carContainer;
    public GameObject[] bikeContainer;
    public GameObject currentCharacter;

    public void setCharacterMode()
    {
        int _characterMode = PlayerPrefs.GetInt("debugCharacterIndex", 1);
        for (int i = 0; i < characterContainer.Length; i++)
        {
            if (i == _characterMode)
            {
                characterContainer[i].SetActive(true);
                setCharacterPlayer(i);
            }
            else
            {
                characterContainer[i].SetActive(false);
            }
        }
    }
    void setCharacterPlayer(int index)
    {

        int _playerIndex = PlayerPrefs.GetInt("playerIndex", 0);
        switch (index)
        {
            case 0:
                for (int i = 0; i < stickmanContainer.Length; i++)
                {
                    if (i == _playerIndex)
                    {
                        stickmanContainer[i].SetActive(true);
                        currentCharacter = stickmanContainer[i];
                    }
                    else
                    {
                        stickmanContainer[i].SetActive(false);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < carContainer.Length; i++)
                {
                    if (i == _playerIndex)
                    {
                        carContainer[i].SetActive(true);
                        currentCharacter = carContainer[i];
                    }
                    else
                    {
                        carContainer[i].SetActive(false);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < bikeContainer.Length; i++)
                {
                    if (i == _playerIndex)
                    {
                        bikeContainer[i].SetActive(true);
                        currentCharacter = bikeContainer[i];
                    }
                    else
                    {
                        bikeContainer[i].SetActive(false);
                    }
                }
                break;
        }
    }
}
