using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class Word{

    public string word;
    private int typeIndex;
    WordDisplay display;


    public Word(string word, WordDisplay display)
    {
        //tallettaa konstruktorin saaman sanan muuttujaan word ja juuri luodun prefabin WordDisplay scriptin muuttujaan display
        //näitä muuttujia käytetään myöhemmin tässä scriptissä
        this.word = word;
        typeIndex = 0;
        this.display = display;
        display.SetWord(word);
    }

    public char GetNextLetter()
    {
        //palauttaa seuraavan kirjaimen sanasta
        return word[typeIndex];
    }

    public void TypeLetter()
    {
        //lisää kirjoitettavan sanan indeksiä ja käskee prefabia (sitä prefabia jossa kyseinen sana on) poistamaan kirjoitetun kirjaimen
        typeIndex++;
        display.RemoveLetter();
        
    }

    public void wrongLetter()
    {
        //kasvattaa väärien inputtien määrää yhdellä
        Stats.wrongInput += 1;

        if (SceneManager.GetActiveScene().name != "Mode1")
        {

            //laittaa edellisen kirjaimen näytölle jos pelaaja kirjoittaa väärän kirjaimen
            if (typeIndex - 1 >= 0)
            {
                typeIndex--;
            }
            display.SetWord(word.Substring(typeIndex));
        }
        
    }


    public bool WordTyped()
    {
        
        bool wordTyped = (typeIndex >= word.Length);
        if (wordTyped)
        {
            //jos koko sana on kirjoitettu kokonaan, ohjelma kutsuu funktiota joka poistaa sanan näytöltä
            display.RemoveWord();
        }
        return wordTyped;
    }

}
