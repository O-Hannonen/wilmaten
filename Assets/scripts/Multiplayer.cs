using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Multiplayer : MonoBehaviour
{

    public List<Word> words;
    private bool hasActiveWord;
    private Word activeWord;
    public WordSpawner wordSpawner;
    public AudioClip correctWord;
    public AudioClip decreaseLives;
    private AudioSource audiosource;
    public AudioSource musicAudiosource;
    public TextMeshProUGUI countDownText;
    public AudioClip countDownAudio;
    public int wordCount;
    private bool started;
    private bool shouldChangeMusic;
    private float timePlayed, startTime, currentTime;
    private int countDownAudioPlayed;
    public GameObject scoreKeeper;
    private multiplayerData data;
    public Transform prefabTransform;

    public List<AudioClip> musics;

    public GameObject gameOver;
    public TextMeshProUGUI thisTimeText, otherTimeText;


    public void bothReady()
    {
        startTime = Time.realtimeSinceStartup;
    }

    private void Awake()
    {
        wordCount = 0;
        GameObject scorekeeper = PhotonNetwork.Instantiate(scoreKeeper.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        scorekeeper.transform.SetParent(prefabTransform, false);
        startTime = 0f;
    }

    public void restartGame()
    {
        startTime = 0f;
        gameOver.SetActive(false);
    }


    private void Start()
    {
        countDownAudioPlayed = 0;
        shouldChangeMusic = false;
        audiosource = gameObject.GetComponent<AudioSource>();
        started = false;
        startTime = 0f;
        gameOver.SetActive(false);
    }

    private void Update()
    {
        if(GameObject.Find("thisPhotonView") == true)
        {

            data = GameObject.Find("thisPhotonView").GetComponent<multiplayerData>();
           

            thisTimeText.text = "Aikasi: " + data.getThisTime() + "s";
            if(data.getOtherTime() == 0)
            {
                otherTimeText.text = "Vastustaja pelaa yhä...";
                
                
            }
            else
            {
                otherTimeText.text = "Vastustajan aika: " + data.getOtherTime() + "s";
              
            }
        }

        if (shouldChangeMusic == true && wordCount <= 9)
        {
            //kun sana on kirjoitettu, shouldChangeMusic laitetaan arvoon true (alempana scriptissä), jolloin musiikki vaihdetaan intensiivisemmäksi
            musicAudiosource.clip = musics[wordCount];
            musicAudiosource.Play();
            shouldChangeMusic = false;
        }

        if(startTime != 0f)
        {
            //startTime on 0 ennen kuin multiplayerData -scriptistä kutsutaan botReady metodia, eli lähtölaskenta ei ala ennenkuin molemmat ovat valmiita
            if (started == false)
            {
                //hoitaa lähtölaskennan

                //pysäyttää ajan
                Time.timeScale = 0f;
                shouldChangeMusic = false;
                float currentTime = Time.realtimeSinceStartup;

                if (currentTime - startTime <= 1f)
                {
                    countDownText.text = "3";
                    if (countDownAudioPlayed == 0)
                    {
                        audiosource.PlayOneShot(countDownAudio);
                        countDownAudioPlayed++;
                    }
                }
                else if (currentTime - startTime <= 2f)
                {
                    countDownText.text = "2";
                    if (countDownAudioPlayed == 1)
                    {
                        audiosource.PlayOneShot(countDownAudio);
                        countDownAudioPlayed++;
                    }
                }
                else if (currentTime - startTime <= 3f)
                {
                    countDownText.text = "1";
                    if (countDownAudioPlayed == 2)
                    {
                        audiosource.PlayOneShot(countDownAudio);
                        countDownAudioPlayed++;
                    }
                }
                else
                {
                    countDownText.text = "";
                    shouldChangeMusic = true;

                    //lisää sanan näytölle lähtölaskennan jälkeen
                    AddWord();
                    started = true;
                }
            }
            else
            {
                //lähtölaskennan aikana aika oli pysäytetty, ja nyt se laitetaan takaisin normaaliksi
                Time.timeScale = 1f;
            }
        }
    }

    public void AddWord()
    {

        //luo uuden word-objektin random sanalla ja spawnaa sen
        Word word = new Word(data.GetNextWord(wordCount), wordSpawner.SpawnWord());
        print("index: " + wordCount);
        //lisää luodun objectin words listaan
        words.Add(word);
    }


    public void destroyed()
    {
        //poistaa vanhimman sanan words listasta
        audiosource.PlayOneShot(decreaseLives);
        words.RemoveAt(0);
        hasActiveWord = false;
    }


    public void TypeLetter(char letter)
    {


        //WordInput scripti kutsuu tätä aliohjelmaa, jolloin letter muuttujaan talletetaan aina pelaajan painama nappi
        //Kaikki tämä suoritetaan joka napin painalluksella tarkistamaan oliko painallus oikea vai ei, mutta suoritus tehdään vain kerran per painallus
        if (hasActiveWord)
        {
            //tämä lohko tapahtuu jos pelaajalla on jonkun sanan kirjoittaminen kesken

            if (activeWord.GetNextLetter() == letter)
            {
                //tämä lohko tapahtuu jos pelaajan näppäimistöllä painama nappi vastaa kirjoitettavan sanan seuraavaa kirjainta
                activeWord.TypeLetter();
                //jos näppäimistön syöte vastasi kirjoitettavan sanan seuraavaa kirjainta, ohjelma kutsuu Word scriptin TypeLetter 
            }
            else if (activeWord.GetNextLetter() != letter)
            {
                activeWord.wrongLetter();
            }
        }
        else
        {
            //tämä lohko suoritetaan jos käyttäjä ei ole aloittanut minkään sanan kirjoittamista

            foreach (Word word in words)
            {
                //ohjelma käy läpi jokaisen Word-olion words listassa, ja tallettaa sen käsittelyn ajaksi muuttujaan word
                if (word.GetNextLetter() == letter)
                {
                    //tämä lohko suoritetaan jos käsiteltävän sanan ensimmäinen kirjain vastaa pelaajan syötettä (näppäimistöllä painamaa merkkiä)


                    //laittaa aktiiviseksi sanaksi tämän sanan
                    activeWord = word;
                    hasActiveWord = true;

                    //poistaa jo kirjoitetun sanan näytöltä
                    word.TypeLetter();

                    //lopettaa words listan tarkastelun, sillä on jo löytynyt word-olio joka laitettiin aktiiviseksi
                    break;
                }
            }
        }



        if (hasActiveWord && activeWord.WordTyped())
        {
            
            //tämä lohko suoritetaan jos kirjoitettavana oleva sana saadaan kirjoitettua kokonaan
            audiosource.PlayOneShot(correctWord);
            Stats.score += 5;
            words.Remove(activeWord);
            data.wordTyped();
            if (wordCount <= 9)
            {
                wordCount++;
                //laittaa seuraavan sanan näkyville
                shouldChangeMusic = true;
                AddWord();
                hasActiveWord = false;
            }
            else
            {
                //jos kymmenen sanaa on kirjoitettu, avataan gameOver scene
                gameOver.SetActive(true);
                musicAudiosource.Pause();
            }

        }



    }

    public void mainMenu()
    {
        data.restartGame();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("mainMenu");
    }
}
