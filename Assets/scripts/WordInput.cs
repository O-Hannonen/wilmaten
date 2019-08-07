using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WordInput : MonoBehaviour {

    public Mode1 mode1;
    public Mode2 mode2;
    public Mode3 mode3;
    public Multiplayer multiplayer;
	void Update () {
		foreach(char letter in Input.inputString)
        {

            //tutkii jokaisen näppäimistön syötteenä tulevan merkin ja kutsuu senhetkisen pelimuodon TypeLetter aliohjelmaa, joka aloittaa tarkistusprosessin(onko kirjain se mitä halutaan)
            if (SceneManager.GetActiveScene().name == "Mode1")
            {
                mode1.TypeLetter(letter);

            }
            else if (SceneManager.GetActiveScene().name == "Mode2")
            {
                mode2.TypeLetter(letter);
            }
            else if (SceneManager.GetActiveScene().name == "Mode3")
            {
                mode3.TypeLetter(letter);
            }
            else if(SceneManager.GetActiveScene().name == "Multiplayer")
            {
                multiplayer.TypeLetter(letter);
            }
        }
	}
}
