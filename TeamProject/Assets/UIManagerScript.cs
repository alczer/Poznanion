using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour {

    public void StartGame()
    {
        //Application.LoadLevel("game");
        SceneManager.LoadScene("game");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("menu");

    }

}
