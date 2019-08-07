using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordSpawner : MonoBehaviour {

    public GameObject wordPrefab;
    public Transform wordHolder;
    
    public WordDisplay SpawnWord()
    {

        //spawnaa word -prefabin näytölle ja palauttaa prefabista wordDisplay scriptin
        Vector3 randomPosition;
        if(SceneManager.GetActiveScene().name != "Mode3" && SceneManager.GetActiveScene().name != "Multiplayer")
        {
           randomPosition = new Vector3(Random.Range(-.5f, .5f), 5.8f);
        }
        else
        {
            randomPosition = new Vector3(0, 0);
        }

       
        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity, wordHolder);
        WordDisplay wordDisplay = wordObj.GetComponent<WordDisplay>();
        return wordDisplay;
    }
}




 