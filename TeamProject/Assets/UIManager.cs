using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    GameManager GM;
    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }
    public void StartGame()
    {
        GM.SetGameState(GameState.GAME);
        Debug.Log(GM.gameState);
    }
    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    public void GoToMenu()
    {
        GM.SetGameState(GameState.MAIN_MENU);
        Debug.Log(GM.gameState);
    }
    public void HandleOnStateChange()
    {
        Debug.Log("CHANGED STATE!");
        
    }



}
