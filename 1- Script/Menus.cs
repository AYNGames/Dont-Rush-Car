using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{


    ObstacleManagment obstacleManagment;



    //[HideInInspector]
    //public bool isOver = false;

    [HideInInspector]
    public GameObject playerGM;
    [HideInInspector]
    public GameObject player2GM;


    public GameObject saveMeMenu;
    public GameObject mainMenu;
    public GameObject soundOff;
    public GameObject gamePlayMenu;
    public GameObject gameOverMenu;
    public GameObject playerControlMenu;
    public GameObject pauseMenu;
    public CameraSmoothFollow cameraFollow;
    public AudioSource buttonClick;


    public Text score;
    public Text resultText;
    public static Menus Static;

    public AudioSource crashSound;
    public Transform finishAreaPos;
    [Header("LEVEL ELEMETS")]
    public int levelDistance = 500;
    public Image levelLoaderImage;
    public Slider levelSlider;
    public Slider levelSliderRival;
    public Text currentLevelText;
    public Text nextLevelText;
    int currentLevel = 1;
    public Text totalMoneyText;
    public Text resulText;
    public string[] resultString;
    [HideInInspector]
   public int obstacle = 0;
    public Text revenueText;
    public int gameStatusIndex = 0;
    // 0 =>main menu
    // 1 => shop
    // 2 => ingame
    // 3 => tapfast
    // 4 => xPointArea
    // 5 => finish

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Static = this;
        Time.timeScale = 1;
        obstacleManagment = GetComponent<ObstacleManagment>();
        gameStatusIndex = 0;

        playerGM = GameObject.Find("PlayerGM");
        if (playerGM != null)
        {
            playerGM.transform.position = new Vector3(0, 0.5f, 0);
        }
        else
        {
            playerGM = Instantiate(Resources.Load("PlayerGM") as GameObject);
            playerGM.transform.position = new Vector3(0, 0.5f, 0);
            playerGM.name = "PlayerCube";
            playerGM.GetComponent<PlayerLogic>().isAi = false;

            player2GM = Instantiate(Resources.Load("PlayerGM") as GameObject);
            player2GM.transform.position = new Vector3(-25, 0.5f, 0);
            player2GM.name = "PlayerCube2P";
            player2GM.GetComponent<PlayerLogic>().isAi = true;
        }

        cameraFollow.enabled = true;
    }
    void setlevelUI()
    {
        if (gameStatusIndex == 2)
        {
            if (playerGM)
            {
                if (playerGM.activeInHierarchy)
                {
                    float _distance = Vector3.Distance(playerGM.transform.position, finishAreaPos.position);
                    levelLoaderImage.fillAmount = _distance / levelDistance;
                    levelSlider.value = _distance / levelDistance;
                }
            }
            if (player2GM)
            {
                if (player2GM.activeInHierarchy)
                {
                    float _distance = Vector3.Distance(player2GM.transform.position, finishAreaPos.position);
                    levelLoaderImage.fillAmount = _distance / levelDistance;
                    levelSliderRival.value = _distance / levelDistance;
                }
            }
        }
    }
    private void LateUpdate()
    {
        setlevelUI();
    }
    private void Start()
    { 
        mainSoundCtrl.Static.chanceSoundVolume(0.5f);
        setInitialCurrentCharacter();
        initalMonetAssigaentFonk();
        setInitialModeInfo();
        initialSpeedUpgarde();

        obstacle = 0;
        for (int i = 0; i < 1; i++)
        {
            obstacleManagment.CreateObstacle(0);
        }

    }


    void initalMonetAssigaentFonk()
    {
        //PlayerPrefs.SetInt("money", 5000);
        totalMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();


    }
    void setMoneyFonk(int _price)
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + _price);
        totalMoneyText.text = PlayerPrefs.GetInt("money", 0).ToString();
    }
    void getLevelUI()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        currentLevelText.text = currentLevel.ToString();
        //currentLevelTextStartScreen.text = "LEVEL " + currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
    }

    public void Play()
    {

        gameStatusIndex = 2;


        int _characterMode = PlayerPrefs.GetInt("debugCharacterIndex", 1);

        if (_characterMode == 0)
        {
            playerGM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>().Play("RUN");
            player2GM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>().Play("RUN");

        }


        getLevelUI();
        buttonClick.Play();
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        score.text = "SCORE: 0";


        for (int i = 0; i < 4; i++)
        {
            obstacleManagment.CreateObstacle(0);
        }
        obstacleManagment.CreateObstacle(1);
        mainMenu.SetActive(false);
        gamePlayMenu.SetActive(true);
        Time.timeScale = 1;
        cameraFollow.enabled = true;      
        initialIslandPos();
    }
   
    public void RePlay()
    {  
        Application.LoadLevel(Application.loadedLevel);        
    }
    void initialIslandPos()
    {
        ileriAtCtrl[] ileriAtCtrls = FindObjectsOfType<ileriAtCtrl>();
        for (int i = 0; i < ileriAtCtrls.Length; i++)
        {
            ileriAtCtrls[i].setInitialPos();
        }
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        buttonClick.Play();
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        cameraFollow.enabled = false;

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacle = 0;

        GameObject playerCube = GameObject.Find("PlayerCube");
        if (playerCube != null) Destroy(playerCube);
        gamePlayMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        buttonClick.Play();
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        Time.timeScale = 1;
        buttonClick.Play();
        pauseMenu.SetActive(false);
    }

    bool youWin = false;
    float revenueXPoint = 1.0f;
    public void ShowGameOverMenu(bool _youWin, float _revenueXPoint)
    {
        revenueXPoint = _revenueXPoint;
        youWin = _youWin;
        GameOver();
    }
    public int levelComplatePrize = 100;
    bool doOnceGameOver = false;

    float calcualteXpoint()
    {
        float _xPoint = 1.0f;
        switch (tapFastCounter)
        {
            case 0:
                _xPoint = 1.0f;
                break;
            case 1:
                _xPoint = 1.1f;
                break;
            case 2:
                _xPoint = 1.2f;
                break;
            case 3:
                _xPoint = 1.3f;
                break;
            case 4:
                _xPoint = 1.4f;
                break;
            case 5:
                _xPoint = 1.5f;
                break;
            case 6:
                _xPoint = 1.6f;
                break;
            case 7:
                _xPoint = 1.7f;
                break;
            case 8:
                _xPoint = 1.8f;
                break;
            case 9:
                _xPoint = 1.9f;
                break;
            case 10:
                _xPoint = 2.0f;
                break;
            case 11:
                _xPoint = 2.1f;
                break;
            case 12:
                _xPoint = 2.2f;
                break;
            case 13:
                _xPoint = 2.3f;
                break;
            case 14:
                _xPoint = 2.4f;
                break;
            case 15:
                _xPoint = 2.5f;
                break;
            case 16:
                _xPoint = 2.6f;
                break;
            case 17:
                _xPoint = 2.7f;
                break;
            case 18:
                _xPoint = 2.8f;
                break;
            case 19:
                _xPoint = 2.9f;
                break;
            case 20:
                _xPoint = 3.0f;
                break;
            case 21:
                _xPoint = 3.1f;
                break;
            case 22:
                _xPoint = 3.2f;
                break;
            case 23:
                _xPoint = 3.3f;
                break;
            case 24:
                _xPoint = 3.4f;
                break;
            case 25:
                _xPoint = 3.5f;
                break;
            case 26:
                _xPoint = 3.6f;
                break;
            case 27:
                _xPoint = 3.7f;
                break;
            case 28:
                _xPoint = 3.8f;
                break;
            case 29:
                _xPoint = 3.9f;
                break;
            case 30:
                _xPoint = 4.0f;
                break;
            case 31:
                _xPoint = 4.1f;
                break;
            default:
                _xPoint = 4.2f;
                break;
        }
        return _xPoint;
    }
    public GameObject gameOverRevenuePanel;
    private void GameOver()
    {
        if (doOnceGameOver)
            return;

        doOnceGameOver = true;
        if (youWin)
        {
            resulText.text = resultString[1];
            float _gameOverMoney = levelComplatePrize * (calcualteXpoint());
            revenueText.text = "+" + _gameOverMoney.ToString();
            setMoneyFonk((int)_gameOverMoney);
        }
        else
        {
            gameOverRevenuePanel.SetActive(false);
            resulText.text = resultString[2];
        }
        gamePlayMenu.SetActive(true);
        playerControlMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void SoundOnOff()
    {
        buttonClick.Play();
        if (AudioListener.volume == 0f)
        {
            AudioListener.volume = 1f;
            soundOff.SetActive(false);
        }
        else
        {
            soundOff.SetActive(true);
            AudioListener.volume = 0f;
        }
    }

    public void Quit()
    {
        buttonClick.Play();
        Application.Quit();
    }

    public void failGameOver()
    {
        FindObjectOfType<PlayerLogic>().gameOverFonk();
    }

    public int speedIndex = 0;
    // 0 => standart
    // 1 => fast
    // 2 =Z slow
    public void speedupButtonFonk(int index)
    {
        if (index == 1)
        {
            speedIndex = 1;
        }
        else
        {
            speedIndex = 0;
        }
    }

    public void speedDownButtonFonk(int index)
    {
        if (index == 1)
        {
            speedIndex = 2;
        }
        else
        {
            speedIndex = 0;
        }
    }


    [Header("SHOP MANAGER")]
    public GameObject shopContainer;
    public GameObject shopMenuGM;
    public GameObject lockImage;
    public GameObject buyBtn;
    public GameObject selectBtn;
    public Text buyButtonPriceText;
    public GameObject[] shopCharacter;
    public int[] prize;
    int _currentCharacter = 0;
    public void openCloseShopMenu()
    {
        if (shopMenuGM.activeInHierarchy)
        {

            gameStatusIndex = 0;

            mainMenu.SetActive(true);
            shopMenuGM.SetActive(false);
            shopContainer.SetActive(false);
            cameraFollow.shopOpenClose(false);
        }
        else
        {
            gameStatusIndex = 1;

            mainMenu.SetActive(false);
            shopMenuGM.SetActive(true);
            shopContainer.SetActive(true);
            cameraFollow.shopOpenClose(true);
        }
    }

    bool isBuyed(int index)
    {

        int _isBuyed = PlayerPrefs.GetInt("ball" + index, 0);

        if (_isBuyed == 0)
        {
            lockImage.SetActive(true);
            return false;
        }
        else
        {
            lockImage.SetActive(false);
            return true;
        }


    }

    void setInitialCurrentCharacter()
    {
        lockImage.SetActive(false);
        PlayerPrefs.SetInt("ball" + 0, 1);
        _currentCharacter = PlayerPrefs.GetInt("playerIndex", 0);
        for (int i = 0; i < shopCharacter.Length; i++)
        {
            if (i == _currentCharacter)
            {
                shopCharacter[i].SetActive(true);
            }
            else
            {
                shopCharacter[i].SetActive(false);
            }
        }

        buyBtn.SetActive(false);
        selectBtn.SetActive(true);
        selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECTED";
        selectBtn.GetComponent<Button>().interactable = false;

    }
    public void buyFonk()
    {
        int _money = PlayerPrefs.GetInt("money", 0);

        if (prize[_currentCharacter] <= _money)
        {
            lockImage.SetActive(false);
            setMoneyFonk(prize[_currentCharacter]);
            PlayerPrefs.SetInt("ball" + _currentCharacter, 1);
            buyBtn.SetActive(false);
            selectBtn.SetActive(true);
            selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECT";
            selectBtn.GetComponent<Button>().interactable = true;

            buyAndUpgradeSound.Play();
        }
    }
    public void selectFonk()
    {
        PlayerPrefs.SetInt("playerIndex", _currentCharacter);
        int _selectedCharacter = PlayerPrefs.GetInt("playerIndex", 0);

        selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECTED";
        selectBtn.GetComponent<Button>().interactable = false;

        buyAndUpgradeSound.Play();
    }
    public void nextCharacter()
    {
        _currentCharacter++;
        if (_currentCharacter >= shopCharacter.Length)
        {
            _currentCharacter = 0;
        }
        for (int i = 0; i < shopCharacter.Length; i++)
        {
            if (i == _currentCharacter)
            {
                shopCharacter[i].SetActive(true);
            }
            else
            {
                shopCharacter[i].SetActive(false);
            }
        }
        buyButtonPriceText.text = prize[_currentCharacter].ToString();

        if (isBuyed(_currentCharacter) == false)
        {
            buyBtn.SetActive(true);
            selectBtn.SetActive(false);
            selectBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            buyBtn.SetActive(false);
            selectBtn.SetActive(true);

            int _selectedCharacter = PlayerPrefs.GetInt("playerIndex", 0);
            if (_currentCharacter == _selectedCharacter)
            {
                selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECTED";
                selectBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECT";
                selectBtn.GetComponent<Button>().interactable = true;
            }
        }



    }

    public void previousCharacter()
    {
        _currentCharacter--;
        if (_currentCharacter < 0)
        {
            _currentCharacter = shopCharacter.Length - 1;
        }
        for (int i = 0; i < shopCharacter.Length; i++)
        {
            if (i == _currentCharacter)
            {
                shopCharacter[i].SetActive(true);
            }
            else
            {
                shopCharacter[i].SetActive(false);
            }
        }

        buyButtonPriceText.text = prize[_currentCharacter].ToString();

        if (isBuyed(_currentCharacter) == false)
        {
            buyBtn.SetActive(true);
            selectBtn.SetActive(false);
            selectBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            buyBtn.SetActive(false);
            selectBtn.SetActive(true);

            int _selectedCharacter = PlayerPrefs.GetInt("playerIndex", 0);
            if (_currentCharacter == _selectedCharacter)
            {
                selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECTED";
                selectBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                selectBtn.transform.GetChild(0).GetComponent<Text>().text = "SELECT";
                selectBtn.GetComponent<Button>().interactable = true;
            }
        }

    }


    [Header("DEBUG ELEMENTS")]
    public GameObject debugCanvas;
    public Text characterText;
    public Text environmentText;
    public Text cameraAngleText;
    public Text skyBoxText;
    public Text shaderText;
    public Image[] characterListImage;
    public Image[] environmentListImage;
    public Image[] cameraAngleListImage;
    public Image[] skyBoxListImage;
    public Image[] shaderListImage;
    public Material[] skyboxMats;
    public string[] characterNames;
    public string[] environmentNames;
    public string[] cameraAngleNames;
    public string[] skyBoxNames;
    public string[] shaderNames;

    int debugCharacterIndex = 0;
    int debugEnvironmentIndex = 0;
    int debugCameraIndex = 0;
    int debugSkyBoxIndex = 0;
    int debugShaderIndex = 0;

    void setInitialModeInfo()
    {
        debugCharacterIndex = PlayerPrefs.GetInt("debugCharacterIndex", 1);
        debugEnvironmentIndex = PlayerPrefs.GetInt("debugEnviIndex", 1);
        debugCameraIndex = PlayerPrefs.GetInt("debugAngleIndex", 0);
        debugSkyBoxIndex = PlayerPrefs.GetInt("debugSkyBoxIndex", 2);

        chanceListImage(debugCharacterIndex, 0);
        chanceListImage(debugEnvironmentIndex, 1);
        chanceListImage(debugCameraIndex, 2);
        chanceListImage(debugSkyBoxIndex, 3);

        characterText.text = characterNames[debugCharacterIndex];
        environmentText.text = environmentNames[debugEnvironmentIndex];
        cameraAngleText.text = cameraAngleNames[debugCameraIndex];
        skyBoxText.text = skyBoxNames[debugSkyBoxIndex];

        RenderSettings.skybox = skyboxMats[debugSkyBoxIndex];
        setShaders();
    }
    public void openCloseDebugMenu()
    {
        if (debugCanvas.activeInHierarchy)
        {
            debugCanvas.SetActive(false);
            FindObjectOfType<environmentCtrl>().chanceEnvironment(debugEnvironmentIndex);
            playerGM.GetComponent<PlayerLogic>().setCharacterMode();
            player2GM.GetComponent<PlayerLogic>().setCharacterMode();

            Time.timeScale = 1;
            AudioListener.volume = 1;

        }
        else
        {
            debugCanvas.SetActive(true);
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
    }
    void chanceListImage(int imageIndex, int modeIndex)
    {

        switch (modeIndex)
        {
            case 0:
                for (int i = 0; i < characterListImage.Length; i++)
                {
                    if (imageIndex == i)
                    {
                        characterListImage[i].color = Color.green;
                    }
                    else
                    {
                        characterListImage[i].color = Color.white;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < environmentListImage.Length; i++)
                {
                    if (imageIndex == i)
                    {
                        environmentListImage[i].color = Color.green;
                    }
                    else
                    {
                        environmentListImage[i].color = Color.white;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < cameraAngleListImage.Length; i++)
                {
                    if (imageIndex == i)
                    {
                        cameraAngleListImage[i].color = Color.green;
                    }
                    else
                    {
                        cameraAngleListImage[i].color = Color.white;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < skyBoxListImage.Length; i++)
                {
                    if (imageIndex == i)
                    {
                        skyBoxListImage[i].color = Color.green;
                    }
                    else
                    {
                        skyBoxListImage[i].color = Color.white;
                    }
                }
                break;
            case 4:
                for (int i = 0; i < shaderListImage.Length; i++)
                {
                    if (imageIndex == i)
                    {
                        shaderListImage[i].color = Color.green;
                    }
                    else
                    {
                        shaderListImage[i].color = Color.white;
                    }
                }
                break;
        }
    }
    public void chanceCharacter(int index)
    {
        if (index == -1) // previous character
        {
            debugCharacterIndex--;
            if (debugCharacterIndex < 0)
            {
                debugCharacterIndex = characterNames.Length - 1;
            }
            characterText.text = characterNames[debugCharacterIndex];

        }
        if (index == 1) // next character
        {
            debugCharacterIndex++;
            if (debugCharacterIndex >= characterNames.Length)
            {
                debugCharacterIndex = 0;
            }
            characterText.text = characterNames[debugCharacterIndex];
        }
        PlayerPrefs.SetInt("debugCharacterIndex", debugCharacterIndex);
        chanceListImage(debugCharacterIndex, 0);
    }

    public void chanceEnvironment(int index)
    {
        if (index == -1) // previous character
        {
            debugEnvironmentIndex--;
            if (debugEnvironmentIndex < 0)
            {
                debugEnvironmentIndex = environmentNames.Length - 1;
            }
            environmentText.text = environmentNames[debugEnvironmentIndex];

        }
        if (index == 1) // next character
        {
            debugEnvironmentIndex++;
            if (debugEnvironmentIndex >= environmentNames.Length)
            {
                debugEnvironmentIndex = 0;
            }
            environmentText.text = environmentNames[debugEnvironmentIndex];
        }
        PlayerPrefs.SetInt("debugEnviIndex", debugEnvironmentIndex);

        chanceListImage(debugEnvironmentIndex, 1);
    }

    public void chanceCameraAngle(int index)
    {
        if (index == -1) // previous character
        {
            debugCameraIndex--;
            if (debugCameraIndex < 0)
            {
                debugCameraIndex = cameraAngleNames.Length - 1;
            }
            cameraAngleText.text = cameraAngleNames[debugCameraIndex];

        }
        if (index == 1) // next character
        {
            debugCameraIndex++;
            if (debugCameraIndex >= environmentNames.Length)
            {
                debugCameraIndex = 0;
            }
            cameraAngleText.text = cameraAngleNames[debugCameraIndex];
        }

        PlayerPrefs.SetInt("debugAngleIndex", debugCameraIndex);
        cameraFollow.chanceCameraIndex();
        chanceListImage(debugCameraIndex, 2);
    }
    public void chanceSkyBox(int index)
    {
        if (index == -1) // previous character
        {
            debugSkyBoxIndex--;
            if (debugSkyBoxIndex < 0)
            {
                debugSkyBoxIndex = skyBoxNames.Length - 1;
            }
            skyBoxText.text = skyBoxNames[debugSkyBoxIndex];

        }
        if (index == 1) // next character
        {
            debugSkyBoxIndex++;
            if (debugSkyBoxIndex >= skyBoxNames.Length)
            {
                debugSkyBoxIndex = 0;
            }
            skyBoxText.text = skyBoxNames[debugSkyBoxIndex];
        }

        PlayerPrefs.SetInt("debugSkyBoxIndex", debugSkyBoxIndex);

        RenderSettings.skybox = skyboxMats[debugSkyBoxIndex];

        chanceListImage(debugSkyBoxIndex, 3);
    }
    public void chanceShaderBox(int index)
    {
        if (index == -1) // previous character
        {
            debugShaderIndex--;
            if (debugShaderIndex < 0)
            {
                debugShaderIndex = shaderNames.Length - 1;
            }
            shaderText.text = shaderNames[debugShaderIndex];

        }
        if (index == 1) // next character
        {
            debugShaderIndex++;
            if (debugShaderIndex >= shaderNames.Length)
            {
                debugShaderIndex = 0;
            }
            shaderText.text = shaderNames[debugShaderIndex];
        }

        PlayerPrefs.SetInt("debugShaderIndex", debugShaderIndex);
        setShaders();


        chanceListImage(debugShaderIndex, 4);
    }
    void setShaders() //NOT USED FOR SOME PROBLEMS
    {
        //Renderer[] allRenderer = FindObjectsOfType<Renderer>();
        //for (int i = 0; i < allRenderer.Length; i++)
        //{
        //    if (allRenderer[i].material.shader == Shader.Find("Mobile/Diffuse") || allRenderer[i].material.shader == Shader.Find("Standard") || allRenderer[i].material.shader == Shader.Find("Toony Colors Pro 2/Mobile"))
        //    {
        //        switch (debugShaderIndex)
        //        {
        //            case 0:
        //                allRenderer[i].material.shader = Shader.Find("Standard");
        //                break;
        //            case 1:
        //                allRenderer[i].material.shader = Shader.Find("Toony Colors Pro 2/Mobile");
        //                break;
        //            case 2:
        //                allRenderer[i].material.shader = Shader.Find("Unlit/Texture");
        //                break;
        //        }
        //    }
        //}
    }
    [Header("TAP FAST ELEMETS")]
    public GameObject tapFastCanvas;
    public Slider tapFastTimeSlider;
    public Text tapFastCounterText;
    public bool isTapFastTime = false;
    public float tapFastTimeDelay = 3;
    [HideInInspector]
    public int tapFastCounter = 0;
    public void openCloseTapFastCanvas(bool result)
    {
        if (result)
        {
            isTapFastTime = true;
            gamePlayMenu.SetActive(false);
            tapFastCanvas.SetActive(true);
        }
        else
        {
            tapFastCanvas.SetActive(false);
        }
    }
    private void Update()
    {
        if (isTapFastTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                buttonClickSound.Play();
                tapFastCounter++;
                tapFastCounterText.text = tapFastCounter.ToString();
            }

            tapFastTimeDelay -= Time.deltaTime;
            tapFastTimeSlider.value = tapFastTimeDelay;
            if (tapFastTimeDelay <= 0)
            {
                gameStatusIndex = 4;
                openCloseTapFastCanvas(false);
                isTapFastTime = false;
                if (playerGM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>())
                    playerGM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>().Play("FAST");

            }
            else
            {
                if (playerGM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>())
                    playerGM.GetComponent<PlayerLogic>().currentCharacter.GetComponent<Animator>().Play("CH");
            }
        }
    }

    [Header("SPEED UPGRADE")]
    public Text speedUpgradeRateText;
    public int speedUpgradePrize = 100;
    void initialSpeedUpgarde()
    {
        speedUpgradeRateText.text = "%" + PlayerPrefs.GetInt("skill_speed_LevEl", 1);
    }
    public void speedUpgradeButtonClick()
    {
        int _money = PlayerPrefs.GetInt("money", 0);
        if (_money >= speedUpgradePrize)
        {
            PlayerPrefs.SetInt("skill_speed_LevEl", PlayerPrefs.GetInt("skill_speed_LevEl", 1) + 1);
            speedUpgradeRateText.text = "%" + PlayerPrefs.GetInt("skill_speed_LevEl", 1);
            setMoneyFonk(-speedUpgradePrize);
            buyAndUpgradeSound.Play();
        }
    }
    [Header("SOUNDS GM")]
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource hitSound;
    public AudioSource xPointSound;
    public AudioSource buttonClickSound;
    public AudioSource buyAndUpgradeSound;
 
    public void hitSoundPlay(float volume)
    {
        hitSound.volume = volume;
        hitSound.Play();
    }
    float xPointCounter = 0;
    public void xpOintSoundPlay()
    {
        xPointCounter = xPointCounter + 0.1f;
        xPointSound.pitch = 0.9f + xPointCounter;
        xPointSound.Play();
    }
}
