using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMainMenu : MonoBehaviour {

    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial_greybox");
    }

}
