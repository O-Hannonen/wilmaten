using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WordDisplay : MonoBehaviour
{

    //jokaisessa word -prefabissa, eli viestissä joka spawnataan näytölle, on tämä scripti kiinni.

    public TextMeshProUGUI text;
    public float fallSpeed;
    public float startSpeed;
    public GameObject wordImage;
    private GameObject oldestClone;
    public Mode1 mode1;
    public Mode2 mode2;
    

    public void SetWord(string word)
    {
        //asettaa word -prefabin tekstikentän tekstiksi halutun sanan
        text.text = word;
    }

    public void addLetter(char c)
    {
        text.text = c.ToString() + text.text;
    }



    public void RemoveLetter()
    {
        //poistaa jo kirjoitetun kirjaimen näytöltä
        Stats.characterAmount += 1;
        text.text = text.text.Remove(0, 1);

        //tekee sanasta ns aktiivisen, eli vaihtaa väriä
        text.color = Color.red;
    }

    public void RemoveWord()
    { 
        //poistaa tämän kyseisen objektin jossa tämä scripti on
        Destroy(gameObject);
    }

 

   

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Mode1")
        {
            mode1 = GameObject.FindGameObjectWithTag("manager").GetComponent<Mode1>();
        }
        else if(SceneManager.GetActiveScene().name == "Mode2")
        {
            mode2 = GameObject.FindGameObjectWithTag("manager").GetComponent<Mode2>();
        }
 
        //kiihdyttää putoamisnopeutta mitä pidemmälle peliä on pelattu

        //asettaa tekstiä ympäröivän tekstikentän koon sopivaksi
        wordImage = gameObject.transform.GetChild(0).gameObject;
        if (gameObject.transform.position.x <= 0f)
        {
            //laittaa tekstikenttä kuvan tekstin ympärille
            //jos teksti on näytön vasemmassa reunassa, pieni nuoli tekstiboksin yläreunassa osoittaa vasemmalle
            //säätää tekstiboksin koon x akselilla tekstin pituuden mukaan
            wordImage.transform.localScale = new Vector3( (0.2f + 0.05f * text.text.Length), -1.2f, 1f);
        }
        else
        {
            //jos teksti on näytön oikeassa reunassa, pieni nuoli tekstiboksin yläreunassa osoittaa oikealle
            //säätää tekstiboksin koon x akselilla tekstin pituuden mukaan
            wordImage.transform.localScale = new Vector3( -(0.2f + 0.05f * text.text.Length), -1.2f, 1f);
        }     
    }

    private void Update()
    {
        //kiihdyttää putoamisnopeutta mitä pidemmälle peliä on pelattu
        fallSpeed = Options.hardness + Time.timeSinceLevelLoad / ((12f / Options.hardness) * 100f);


        if (SceneManager.GetActiveScene().name != "Mode3" && SceneManager.GetActiveScene().name != "Multiplayer")
        {
            if (this.transform.position.y >= -5f)
            {
                //liikuttaa sanaa alareunaa kohti nopeudella fallSpeed
                transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
            }
            else
            {
                //jos kirjoitettavana oleva prefabi osuu näytön alareunaan, sen kirjoittaminen lopetataan, se tuhotaan ja elämiä vähennetään
                Stats.lives -= 1;
                if (SceneManager.GetActiveScene().name == "Mode1")
                {
                    mode1.destroyed();
                }
                else if (SceneManager.GetActiveScene().name == "Mode2")
                {
                    mode2.destroyed();
                }

                Destroy(this.gameObject);
            }
        }


    }

}