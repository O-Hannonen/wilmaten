using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordTimer : MonoBehaviour {

    public Mode1 mode1;
    public Mode2 mode2;
    public float wordDelay;       
    private float nextWordTime = 0f;



    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "Mode3" && Time.timeScale != 0f)
        {
            wordDelay = (0.8f / Options.hardness) * 10f;
            if (Time.timeSinceLevelLoad >= nextWordTime)
            {
                //kutsuu säännöllisesti AddWord funktiota, joka ottaa spawnaa uuden sanan näytön yläreunaan

                if (SceneManager.GetActiveScene().name == "Mode1")
                {
                    mode1.AddWord();
                }
                else if (SceneManager.GetActiveScene().name == "Mode2")
                {
                    mode2.AddWord();
                }
                nextWordTime = Time.timeSinceLevelLoad + wordDelay;

            }
        }
    }
}
