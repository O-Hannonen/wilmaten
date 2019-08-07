using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextTutorial : MonoBehaviour {

    public void next()
    {
        SceneManager.LoadScene(4);        
    }
}
