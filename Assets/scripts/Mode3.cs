using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Mode3 : MonoBehaviour
{

    public List<Word> words;
    private bool hasActiveWord;
    private Word activeWord;
    public WordSpawner wordSpawner;
    public AudioClip correctWord;
    public AudioClip decreaseLives;
    private AudioSource audiosource;
    public AudioSource musicAudioSource;
    private bool pauseMusic;
    public TextMeshProUGUI countDownText;
    public AudioClip countDownAudio;
    public int wordCount;
    private bool started;
    private bool shouldChangeMusic;
    private float timePlayed, startTime, currentTime;
    private int countDownAudioPlayed;

    public List<AudioClip> musics;


    public bool paused;

    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject PauseMenu;

    public GameObject progressBar;
    public Scrollbar progressScrollbar;
    private Animation flyingAnimation;



    private void Start()
    {
        flyingAnimation = progressBar.GetComponent<Animation>();
        pauseMusic = true;
        paused = false;
        countDownAudioPlayed = 0;
        shouldChangeMusic = false;
        wordCount = 0;
        audiosource = gameObject.GetComponent<AudioSource>();
        started = false;
        startTime = Time.realtimeSinceStartup;
        PauseMenu.SetActive(false);

    }

    private void Update()
    {
        if(wordCount == 0)
        {
            progressScrollbar.value = 0;
        }
        else
        {
            progressScrollbar.value = wordCount * (1f / 10f);
        }
       
        if (pauseMusic == true)
        {
            musicAudioSource.Pause();
        }
        else
        {
            if (musicAudioSource.isPlaying == false)
            {
                musicAudioSource.Play();
            }
        }

        if (started == true)
        {
            if (Input.GetKeyDown("space"))
            {
                if (paused)
                {
                    playButtonPress();
                }
                else
                {
                    pauseButtonPress();
                }
            }

            if (paused == false)
            {
                pauseButton.SetActive(true);
                playButton.SetActive(false);
                PauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                PauseMenu.SetActive(true);
                pauseButton.SetActive(false);
                playButton.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            pauseButton.SetActive(false);
            playButton.SetActive(false);
            PauseMenu.SetActive(false);
        }


        if (shouldChangeMusic == true && wordCount <= 9)
        {
            //kun sana on kirjoitettu, shouldChangeMusic laitetaan arvoon true (alempana scriptissä), jolloin musiikki vaihdetaan intensiivisemmäksi
            musicAudioSource.clip = musics[wordCount];
            musicAudioSource.Play();
            shouldChangeMusic = false;
        }

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
                pauseMusic = false;
            }
        }
        else
        {
            //lähtölaskennan aikana aika oli pysäytetty, ja nyt se laitetaan takaisin normaaliksi
            if(paused == false)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void AddWord()
    {
        
        //luo uuden word-objektin random sanalla ja spawnaa sen
        Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord());

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


       if(paused == false)
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
                    else if (words.IndexOf(word) == words.Count - 1 && word.GetNextLetter() != letter)
                    {
                        Stats.wrongInput += 1;
                    }
                }
            }



            if (hasActiveWord && activeWord.WordTyped())
            {
                //tämä lohko suoritetaan jos kirjoitettavana oleva sana saadaan kirjoitettua kokonaan
                audiosource.PlayOneShot(correctWord);
                Stats.score += 5;
                words.Remove(activeWord);
                if (wordCount <= 9)
                {
                    //laittaa seuraavan sanan näkyville
                    shouldChangeMusic = true;
                    AddWord();
                    hasActiveWord = false;
                    wordCount++;
                }
                else
                {
                    //jos kymmenen sanaa on kirjoitettu, avataan gameOver scene
                    SceneManager.LoadScene("gameOver");
                }

            }
        }



    }

    public void pauseButtonPress()
    {
        paused = true;
        pauseMusic = true;
    }

    public void playButtonPress()
    {
        paused = false;
        pauseMusic = false;
    }

    public void onScrollbarValueChanged()
    {
        flyingAnimation.Play("flying");
    }
}
