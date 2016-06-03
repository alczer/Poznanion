using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Advertisements;
using System.Collections;

// Game States
public enum GameState { MAIN_MENU, ADD_PLAYER_MENU, PAUSED, GAME, LOBBY, LANGAME, CREDITS, HELP }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    public string zoneId;

    public List<Player> playersList = new List<Player>();
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

    public Player GetRedPlayerCopy()
    {
        return playersList.Find(a => a.color == PlayerColor.RED);
    }
    public Player GetGreenPlayerCopy()
    {
        return playersList.Find(a => a.color == PlayerColor.GREEN);
    }
    public Player GetBluePlayerCopy()
    {
        return playersList.Find(a => a.color == PlayerColor.BLUE);
    }

    public Player GetYellowPlayerCopy()
    {
        return playersList.Find(a => a.color == PlayerColor.YELLOW);
    }

    public Player GetBlackPlayerCopy()
    {
        return playersList.Find(a => a.color == PlayerColor.BLACK);
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
    public void AddScore(PlayerColor color, int score)
    {
        //Debug.Log("liczba graczy: " + playersList.Count);
        //Debug.Log("Lista graczy : " + String.Join(" ", playersList.Select(item => item.color.ToString()).ToArray())); //////////////////////
        //Debug.Log("Lista graczy : " + String.Join(" ", playersList.Select(item => item.name.ToString()).ToArray())); //////////////////////
        playersList.Find(a => a.color == color).ChangeScore(score);
    }

    public void ReturnMeeple(PlayerColor color)
    {
        playersList.Find(a => a.color == color).meeples++;
    }

    public List<Player> GetPlayerListCopy()
    {
        return playersList;
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
                ShowAdPlacement();
                SceneManager.LoadScene("addPlayer");
                break;
            case GameState.LOBBY:
                ShowAdPlacement();
                SceneManager.LoadScene("lanLobby");
                break;
            case GameState.LANGAME:
                break;
            case GameState.CREDITS:
                break;
            case GameState.HELP:
                SceneManager.LoadScene("rules");
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

    public void ShowAdPlacement()
    {
        //Advertisement.debugLevel = Advertisement.DebugLevel.Debug;
        //if(Advertisement.testMode)
        //{
        //    Debug.Log("test");
        //}
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show();
        //}
        //if (string.IsNullOrEmpty(zoneId)) zoneId = null;

        //ShowOptions options = new ShowOptions();
        //options.resultCallback = HandleShowResult;
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show(zoneId, options);
        //}
    }
}
