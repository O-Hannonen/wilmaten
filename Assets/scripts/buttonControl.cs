using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonControl : MonoBehaviour {

    public GameObject firstTutorial;
    public GameObject secondTutorial;
    public static string lastScene;

    public static bool mode1ShouldShowTutorial = true;
    public static bool mode2ShouldShowTutorial = true;
    public static bool mode3ShouldShowTutorial = true;

    public List<Button> buttons;
    public int index;

    public Button tutorialNextButton;
    public Button tutorialPlayButton;

    private int startInt;
    private int endInt;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "TypingTutorial")
        {
            tutorialNextButton.Select();
        }

        index = 0;
        if (SceneManager.GetActiveScene().name != "TypingTutorial")
        {
            startInt = 0;
            endInt = buttons.Count;
        }
        else
        {
            startInt = 0;
            endInt = 2;
            index = endInt - 1;
        }
    }
    private void Update()
    {   
        if(SceneManager.GetActiveScene().name != "options")
        {
            lastScene = SceneManager.GetActiveScene().name;
        }

        if(buttons.Count != 0)
        {
            if(SceneManager.GetActiveScene().name != "TypingTutorial")
            {
                if (Input.GetKeyDown("up"))
                {
                    if (index > startInt)
                    {
                        index--;
                    }
                }
                else if (Input.GetKeyDown("down"))
                {
                    if (index < endInt - 1)
                    {
                        index++;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown("left"))
                {
                    if (index > startInt)
                    {
                        index--;
                    }
                }
                else if (Input.GetKeyDown("right"))
                {
                    if (index < endInt - 1)
                    {
                        index++;
                    }
                }
            }
            

            buttons[index].Select();

            List<Button> list = buttons.GetRange(startInt, endInt - startInt);

            foreach(Button btn in list)
            {
                Image img = btn.GetComponentInParent<Image>();
                if (buttons.IndexOf(btn) == index)
                {
                    img.color = getColor(5080064);
                    
                }
                else
                {
                    img.color = getColor(9810539);
                }
            }
        }

    }

    public Color32 getColor(int hexval)
    {
        byte R = (byte)((hexval >> 16) & 0xFF);
        byte G = (byte)((hexval >> 8) & 0xFF);
        byte B = (byte)((hexval) & 0xFF);

        return new Color32(R, G, B, 255);
    }


    public void nextTutorial()
    {
        firstTutorial.SetActive(false);
        secondTutorial.SetActive(true);
        tutorialPlayButton.Select();
        startInt = 2;
        endInt = 4;
        index = endInt-1;
        
    }

    public void lastTutorial()
    {
        firstTutorial.SetActive(true);
        secondTutorial.SetActive(false);
        startInt = 0;
        endInt = 2;
        index = endInt-1;
    }

    public void openTutorial()
    {
        SceneManager.LoadScene("TypingTutorial");
    }

    public void singlePlayer()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void options()
    {
        SceneManager.LoadScene("options");
    }

    public void backFromOptions()
    {
        SceneManager.LoadScene(lastScene);
    }

    public void mode1()
    {
        if (mode1ShouldShowTutorial)
        {
            mode1ShouldShowTutorial = false;
            SceneManager.LoadScene("Mode1Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Mode1");
        }
        
    }

    public void mode2()
    {
        if (mode2ShouldShowTutorial)
        {
            mode2ShouldShowTutorial = false;
            SceneManager.LoadScene("Mode2Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Mode2");
        }
    }

    public void mode3()
    {
        if (mode3ShouldShowTutorial)
        {
            mode3ShouldShowTutorial = false;
            SceneManager.LoadScene("Mode3Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Mode3");
        }
    }

    public void menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void musicOnOff(bool on)
    {
        //options menun musiikki päälle ja pois toimii tällä
        Options.musicOn = on;
    }

    public void hardnessAdjust(Slider slider)
    {
        Options.hardness = slider.value;
    }

    public void quit()
    {
        Application.Quit();
    }

    public void playAgain()
    {
        SceneManager.LoadScene(Stats.lastGameMode);
    }


}
