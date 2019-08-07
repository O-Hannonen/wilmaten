using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public static float hardness = 2f;

    public static bool musicOn = true;
    public static bool lastOnOff = true;
    public AudioSource music;
    public Toggle musicOnOff;


    public Slider hardnessSlider;

    private void Start()
    {
        if (musicOnOff != null && hardnessSlider != null)
        {
            musicOnOff.isOn = musicOn;
            hardnessSlider.value = hardness;
        }

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "options")
        {
            music.enabled = musicOn;
            if(music.enabled == true)
            {
                if (lastOnOff != musicOn)
                {
                    
                    music.Play();
                    lastOnOff = musicOn;
                }
            }
            else
            {
                if(lastOnOff != musicOn)
                {
                    lastOnOff = musicOn;
                }
            }
            
        }
    }  
}
