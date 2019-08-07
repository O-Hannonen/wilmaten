using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public static int score = 0;

    public TextMeshProUGUI livesText;
    public static int lives = 3;

    public TextMeshProUGUI cpmText;
    public TextMeshProUGUI accuracyText;
    public static float characterAmount;
    public static float timePlayed;
    public static float cpm;
    public static float wrongInput;
    public static float accuracy;

    public static bool shouldShowTime;

    public static string lastGameMode;
    public multiplayerData data;

   





    private void Awake()
    {

        if (SceneManager.GetActiveScene().name != "gameOver")
        {
  
            //aina kun avautuu uusi scene (paitsi jos scene on gameOver), scenen nimi tallennetaan lastGameMode muuttujaan
            //tällöin gameOver scenessä voidaan avata edellinen pelimuoto lastGameMode stringin avulla
            //avaaminen tapahtuu buttonControl scriptissä
            lastGameMode = SceneManager.GetActiveScene().name;


            scoreText = GameObject.FindGameObjectWithTag("score").GetComponent<TextMeshProUGUI>();
            livesText = GameObject.FindGameObjectWithTag("lives").GetComponent<TextMeshProUGUI>();
            cpmText = GameObject.FindGameObjectWithTag("cpm").GetComponent<TextMeshProUGUI>();
            score = 0;
            characterAmount = 0;
            wrongInput = 0;
            lives = 3;

            if (SceneManager.GetActiveScene().name == "Mode3" || SceneManager.GetActiveScene().name == "Multiplayer")
            {
                //mode3 pelin jälkeen halutaan näyttää aika pisteiden tilalla
                shouldShowTime = true;
            }
            else
            {
                shouldShowTime = false;
            }
        }
        else
        {
            scoreText = GameObject.FindGameObjectWithTag("score").GetComponent<TextMeshProUGUI>();
            cpmText = GameObject.FindGameObjectWithTag("cpm").GetComponent<TextMeshProUGUI>();
        }

        //edellisen scenen aukioloaika
        timePlayed = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        

        if(SceneManager.GetActiveScene().name != "gameOver")
        {

            if (shouldShowTime)
            {
                //jos kyseessä on pelimuoto 3 tai multiplayer, scoren kohdalla näytetään aikaa pelin aloituksesta
                livesText.text = "";
                float time = Time.timeSinceLevelLoad;

                

                if(GameObject.Find("thisPhotonView") == true)
                {
                    data = GameObject.Find("thisPhotonView").GetComponent<multiplayerData>();
                    if(data.getThisTime() != 0 && data.getOtherTime() != 0)
                    {
                        scoreText.text = "";
                        Time.timeScale = 0;
                    }
                    else
                    {
                        //mathf.round pyöristää lähimpään tasalukuun, joten näin saadaan desimaalitkin mukaan
                        scoreText.text = time != 0 ? (Mathf.Round(time * 100f) / 100).ToString() : "";
                    }
                }
                else
                {
                    //mathf.round pyöristää lähimpään tasalukuun, joten näin saadaan desimaalitkin mukaan
                    scoreText.text = time != 0 ? (Mathf.Round(time * 100f) / 100).ToString() : "";
                }


                

            }
            else
            {
                scoreText.text = "Pisteet: " + score.ToString();
                livesText.text = "Elämiä: " + lives.ToString();
                if (lives <= 0f)
                {
                    //jos elämiä ei ole jäljellä, peli loppuu ja näytölle ilmestyy peli ohi scene
                    SceneManager.LoadScene("gameOver");
                }
            }

        }else{

            accuracyText = GameObject.FindGameObjectWithTag("accuracy").GetComponent<TextMeshProUGUI>();

            if (shouldShowTime)
            {
                //jos pelattu pelimuoto oli muoto3, niin näyttää pisteiden paikalla ajan
                scoreText.text = "Aika: " + Mathf.Round(timePlayed * 100) / 100 + "s";

            }
            else
            {
                scoreText.text = "Pisteet: " + score.ToString();
            }

            if (timePlayed != 0)
            {
                //timePlayed on sekunteissa, eli se pitää jakaa kuudellakymmenellä jotta saadaan minuutteja
                //sitten siitä voi laskea merkkiä/minuutti
                cpm = Mathf.Round(characterAmount / (timePlayed / 60));
                cpmText.text = "mrk/min: " + cpm.ToString();
            }

            if (characterAmount != 0)
            {
                accuracy = (characterAmount / (characterAmount + wrongInput)) * 100;
                accuracyText.text = "Tarkkuus: " + Mathf.Round(accuracy).ToString() + "%";
            }
            else
            {
                accuracyText.text = "Tarkkuus: 0%";
            }
            
            
        }
    }



    
}
