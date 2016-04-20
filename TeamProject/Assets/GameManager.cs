using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Game States
public enum GameState {MAIN_MENU,ADD_PLAYER_MENU, PAUSED, GAME, CREDITS, HELP }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    private List<Player> playersList = new List<Player>();
    private int currentPlayer = 0; 
    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject g = new GameObject("GameManager");//new GameManager();
                instance = g.AddComponent<GameManager>();
                DontDestroyOnLoad(g);
            }
            return GameManager.instance;
        }
    }
    public void AddPlayer(Player player)
    {
        playersList.Add(player);
    }
    public Player GetCurrentPlayer()
    {
        return playersList[currentPlayer];
    }
    public void NextPlayer()
    {
        if (currentPlayer == playersList.Count - 1)
        {
            currentPlayer = 0;
        }
        else
        {
            currentPlayer++;
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
            case GameState.ADD_PLAYER_MENU:
                SceneManager.LoadScene("addPlayer");
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
