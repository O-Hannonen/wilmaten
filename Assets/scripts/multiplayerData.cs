using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class multiplayerData : Photon.MonoBehaviour {

    public PhotonView photonview;
    public static bool thisReady, otherReady, bothReady;
    public static float thisTime, otherTime;
    public static int thisWordsLeft, otherWordsLeft;
    public static string[] wordList = new string[11];
    public static bool thisTimeGiven, otherTimeGiven;
    public static bool bothReadyCalled;
    public bool otherPhotonViewFound;

    private Multiplayer multiplayer;

    public GameObject waitingUI;
    public TextMeshProUGUI infoText;
    public GameObject readyButton;

    public GameObject leaveButton;


    public GameObject progressBar;
    public Scrollbar progressScrollbar;

    private Animation flyingAnimation;

    public TextMeshProUGUI playerName;

    public void restartGame()
    {
        leaveButton.SetActive(true);
        multiplayer.restartGame();
        Time.timeScale = 0f;
        bothReadyCalled = false;
        thisReady = false;
        otherReady = false;
        bothReady = false;
        thisTimeGiven = false;
        otherTimeGiven = false;
        thisTime = 0f;
        otherTime = 0f;
        thisWordsLeft = 11;
        otherWordsLeft = 11;
        
        
    }

    private void Awake()
    {
        leaveButton.SetActive(true);
        flyingAnimation = progressBar.GetComponent<Animation>();
        Time.timeScale = 0f;
        bothReadyCalled = false;
        thisTimeGiven = false;
        otherTimeGiven = false;
        bothReady = false;
        thisTime = 0f;
        thisWordsLeft = 11;
        otherWordsLeft = 11;
        readyButton.SetActive(false);
        multiplayer = GameObject.FindGameObjectWithTag("manager").GetComponent<Multiplayer>();

        if (PhotonNetwork.isMasterClient == true)
        {
            for (int i = 0; i < 11; i++)
            {
                //pelaaja joka loi huoneen (masterClient) luo molemmille pelaajille yhteisen sanalistan satunnaisista sanoista sanalistassa
                wordList[i] = WordGenerator.GetRandomWord().ToString();
            }

        }
    }



    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        //hoitaa datan lähettämisen

        if (stream.isWriting)
        {
            //määrittää mitä lähtevälle datalle tehdään

            
            stream.SendNext(thisTime);
            stream.SendNext(thisWordsLeft);
            stream.SendNext(thisReady);

            if (PhotonNetwork.isMasterClient == true)
            {
                //masterClient lähettää sanalistansa toiselle
                stream.SendNext(wordList);
            }
        }
        else
        {
            //määrittää mitä saapuvalle datalle tehdään

            
            otherTime = (float)stream.ReceiveNext();
            otherWordsLeft = (int)stream.ReceiveNext();
            otherReady = (bool)stream.ReceiveNext();

            if (PhotonNetwork.isMasterClient == false)
            {
                //vastaanottaa masterClientin luoman sanalistan
                wordList = (string[])stream.ReceiveNext();
            }
        }
    }



    void Update () {
		if(photonview.isMine == true)
        {
            if (thisReady)
            {
                leaveButton.SetActive(false);
            }
            otherPhotonViewFound = GameObject.Find("otherPhotonView");

            playerName.text = "Minä";
            this.name = "thisPhotonView";

            if(otherPhotonViewFound == false && otherWordsLeft != 0)
            {
                //kun toinen pelaaja ei ole vielä saapunut lobbyyn
                infoText.text = "Vastustaja ei ole paikalla...";
                waitingUI.SetActive(true);
            }

            if (otherReady == false && thisReady == true)
            {
                if (otherPhotonViewFound == true)
                {
                    waitingUI.SetActive(true);
                    infoText.text = "Odotetaan että vastustaja on valmis...";

                }

            }

            if (thisReady == false && otherPhotonViewFound == true)
            {

               readyButton.SetActive(true);
               waitingUI.SetActive(true);
               infoText.text = "Paina 'valmis'";
                
            }
            else
            {
                readyButton.SetActive(false);
            }

            if(thisReady == true && otherReady == true)
            {
                bothReady = true;
                waitingUI.SetActive(false);
            }

            if(bothReady == true && bothReadyCalled == false)
            {
                multiplayer.bothReady();
                bothReadyCalled = true;
            }

            if(thisWordsLeft == 0 && thisTimeGiven == false)
            {
                thisTime = getTime();
                thisTimeGiven = true;
                
            }

            progressBar.transform.localPosition = new Vector3(6f,0f,0f);

            progressScrollbar.value = (1f / 11f) * (11 - thisWordsLeft);
            
            
            
            
 
        }
        if(photonview.isMine == false)
        {
            leaveButton.SetActive(false);
            playerName.text = "Vastustaja";
            this.name = "otherPhotonView";
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            progressBar.transform.localPosition = new Vector3(-6f, 0f, 0f);
            progressScrollbar.value = (1f/11f) * (11 - otherWordsLeft);
        }
	}

    public void readyButtonPress()
    {
        thisReady = true;
        readyButton.SetActive(false);
    }

    private float getTime()
    {
        float time =  Time.timeSinceLevelLoad;
        return time != 0 ? Mathf.Round(time * 100f) / 100f : 0;
    }

    private void OnDisconnectedFromPhoton()
    {
        //jos yhteys katkeaa, lataa valikon
        SceneManager.LoadScene("mainMenu");
    }

    public string GetNextWord(int index)
    {
        return wordList[index];
    }

    public float getThisTime()
    {
        return thisTime;
    }

    public float getOtherTime()
    {
        return otherTime;
    }

    public void wordTyped()
    {
        thisWordsLeft--;
    }

    public void onScrollbarValueChanged()
    {
        flyingAnimation.Play("flying");
    }

    public void leaveButtonPress()
    {
        multiplayer.mainMenu();
    }
    
}
