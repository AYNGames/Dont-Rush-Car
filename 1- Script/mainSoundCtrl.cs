using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainSoundCtrl : MonoBehaviour
{

    public static mainSoundCtrl Static;
    AudioSource mainSound;
    private void Awake()
    {
        if(Static == null)
        {
            Static = this;
            DontDestroyOnLoad(this.gameObject);
            mainSound = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void chanceSoundVolume(float _volume)
    {

        mainSound.volume = _volume;


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
