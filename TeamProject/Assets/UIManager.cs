using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

enum PlayerOptions
{
    PLAYER_EMPTY,
    PLAYER_PLAYER,
    PLAYER_AI,
    PLAYER_END
};
public class UIManager : MonoBehaviour
{
    GameManager GM;
    PlayerOptions playerOptionsRed;
    PlayerOptions playerOptionsGreen;
    PlayerOptions playerOptionsBlue;
    PlayerOptions playerOptionsYellow;
    PlayerOptions playerOptionsBlack;
    public GameObject RedFrame;
    public GameObject RedInput;
    public GameObject RedAI;
    public GameObject GreenFrame;
    public GameObject GreenInput;
    public GameObject GreenAI;
    public GameObject BlueFrame;
    public GameObject BlueInput;
    public GameObject BlueAI;
    public GameObject YellowFrame;
    public GameObject YellowInput;
    public GameObject YellowAI;
    public GameObject BlackFrame;
    public GameObject BlackInput;
    public GameObject BlackAI;
    public GameObject StartGameButton;
    
    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
        playerOptionsRed = PlayerOptions.PLAYER_PLAYER;
        playerOptionsGreen = PlayerOptions.PLAYER_EMPTY;
        playerOptionsBlue = PlayerOptions.PLAYER_EMPTY;
        playerOptionsYellow = PlayerOptions.PLAYER_EMPTY;
        playerOptionsBlack = PlayerOptions.PLAYER_EMPTY;
    }
    public void StartGame()
    {
        if(RedInput.activeSelf == true)
        {
            InputField inputField = RedInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Czerwony gracz";
            }
            GM.AddPlayer(new Player(inputField.text,PlayerColor.RED,new Color(1, 0, 0, 1)));
        }
        if (GreenInput.activeSelf == true)
        {
            InputField inputField = GreenInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Zielony gracz";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.GREEN,new Color(0, 1, 0, 1)));
        }
        if (BlueInput.activeSelf == true)
        {
            InputField inputField = BlueInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Niebieski gracz";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.BLUE,new Color(0, 0, 1, 1)));
        }
        if (YellowInput.activeSelf == true)
        {
            InputField inputField = YellowInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Żółty gracz";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.YELLOW,new Color(1, (float)0.92, (float)0.016, 1)));
        }
        if (BlackInput.activeSelf == true)
        {
            InputField inputField = BlackInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Czarny gracz";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.BLACK,new Color(0, 0, 0, 1)));
        }

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
    public void GoToAddPlayerMenu()
    {
        GM.SetGameState(GameState.ADD_PLAYER_MENU);
        Debug.Log(GM.gameState);
    }
    private void HandlePlayerOptions(PlayerOptions options,ref GameObject frame,ref GameObject input, ref GameObject AI)
    {
        switch(options)
        {
            case PlayerOptions.PLAYER_AI:
                Debug.Log("aktywny komputer");
                frame.SetActive(false);
                input.SetActive(false);
                AI.SetActive(true);
                break;
            case PlayerOptions.PLAYER_EMPTY:
                Debug.Log("aktywny nic");
                frame.SetActive(false);
                input.SetActive(false);
                AI.SetActive(false);
                break;
            case PlayerOptions.PLAYER_PLAYER:
                Debug.Log("aktywny player");
                frame.SetActive(true);
                input.SetActive(true);
                AI.SetActive(false);
                break;
        }
    }
    private void CheckStartGamePossibility()
    {
        if (playerOptionsRed == PlayerOptions.PLAYER_PLAYER || playerOptionsGreen == PlayerOptions.PLAYER_PLAYER || playerOptionsBlue == PlayerOptions.PLAYER_PLAYER || playerOptionsYellow == PlayerOptions.PLAYER_PLAYER || playerOptionsBlack == PlayerOptions.PLAYER_PLAYER)
        {
            StartGameButton.SetActive(true);
        }
        else
        {
            StartGameButton.SetActive(false);
        }
    }
    public void NextPlayerOptionRed()
    {
        playerOptionsRed++;
        if (playerOptionsRed == PlayerOptions.PLAYER_END)
        {
            playerOptionsRed = 0;
        }
        HandlePlayerOptions(playerOptionsRed,ref RedFrame,ref RedInput,ref RedAI);
        CheckStartGamePossibility();
    }
    public void NextPlayerOptionGreen()
    {
        playerOptionsGreen++;
        if (playerOptionsGreen == PlayerOptions.PLAYER_END)
        {
            playerOptionsGreen = 0;
        }
        HandlePlayerOptions(playerOptionsGreen, ref GreenFrame, ref GreenInput, ref GreenAI);
        CheckStartGamePossibility();
    }
    public void NextPlayerOptionBlue()
    {
        playerOptionsBlue++;
        if (playerOptionsBlue == PlayerOptions.PLAYER_END)
        {
            playerOptionsBlue = 0;
        }
        HandlePlayerOptions(playerOptionsBlue, ref BlueFrame, ref BlueInput, ref BlueAI);
        CheckStartGamePossibility();
    }
    public void NextPlayerOptionYellow()
    {
        playerOptionsYellow++;
        if (playerOptionsYellow == PlayerOptions.PLAYER_END)
        {
            playerOptionsYellow = 0;
        }
        HandlePlayerOptions(playerOptionsYellow, ref YellowFrame, ref YellowInput, ref YellowAI);
        CheckStartGamePossibility();
    }
    public void NextPlayerOptionBlack()
    {
        playerOptionsBlack++;
        if (playerOptionsBlack == PlayerOptions.PLAYER_END)
        {
            playerOptionsBlack = 0;
        }
        HandlePlayerOptions(playerOptionsBlack, ref BlackFrame, ref BlackInput, ref BlackAI);
        CheckStartGamePossibility();
    }


    public void HandleOnStateChange()
    {
        Debug.Log("CHANGED STATE!");

    }
}



