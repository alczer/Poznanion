using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Game States
public enum GameState {MAIN_MENU, PAUSED, GAME, CREDITS, HELP }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    
    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance == null)
            {
                GameManager.instance = new GameManager();
                DontDestroyOnLoad(GameManager.instance);
            }
            return GameManager.instance;
        }

    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        switch (gameState)
        {
            case GameState.MAIN_MENU:
                SceneManager.LoadScene("menu");
                break;
            case GameState.GAME:
                SceneManager.LoadScene("game");
                break;
            case GameState.CREDITS:                
                break;
            case GameState.HELP:
                break;
            case GameState.PAUSED:
                break;
        }
               
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }
   


}

