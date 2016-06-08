using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

enum PlayerOptions
{
    PLAYER_EMPTY,
    PLAYER_PLAYER,
    PLAYER_AI,
    PLAYER_END
};

public class UIManager : NetworkBehaviour
{
    public NetworkView networkView;
    public int player;
    public GameManager GM;
    Ads ADS;
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
    public int Client_number;
    public int Connenctions_number;
    public bool[] Connections;
    public int group;
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
    public int wait;
    public bool change;
    public void StartGame()
    {
        if ((Network.connections.Length > 0) && (Network.peerType == NetworkPeerType.Server))
        {
            networkView.RPC("LanStartGame", RPCMode.All);

        }
        GM.playersList = new List<Player>();
        if (RedInput.activeSelf == true)
        {
            InputField inputField = RedInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Czerwony";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.RED, new Color(1, 0, 0, 1)));
        }
        if (GreenInput.activeSelf == true)
        {
            InputField inputField = GreenInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Zielony";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.GREEN, new Color(0, 1, 0, 1)));
        }
        if (BlueInput.activeSelf == true)
        {
            InputField inputField = BlueInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Niebieski";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.BLUE, new Color(0, 0, 1, 1)));
        }
        if (YellowInput.activeSelf == true)
        {
            InputField inputField = YellowInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Żółty";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.YELLOW, new Color(1, (float)0.92, (float)0.016, 1)));
        }
        if (BlackInput.activeSelf == true)
        {
            InputField inputField = BlackInput.GetComponent<InputField>();
            if (string.IsNullOrEmpty(inputField.text))
            {
                inputField.text = "Czarny";
            }
            GM.AddPlayer(new Player(inputField.text, PlayerColor.BLACK, new Color(0, 0, 0, 1)));
        }

        ////
        if (RedAI.activeSelf == true)
        {
            GM.AddPlayer(new Player("AI1", PlayerColor.RED, new Color(1, 0, 0, 1), PlayerType.AI));
        }
        if (GreenAI.activeSelf == true)
        {
            GM.AddPlayer(new Player("AI2", PlayerColor.GREEN, new Color(0, 1, 0, 1), PlayerType.AI));
        }
        if (BlueAI.activeSelf == true)
        {
            GM.AddPlayer(new Player("AI3", PlayerColor.BLUE, new Color(0, 0, 1, 1), PlayerType.AI));
        }
        if (YellowAI.activeSelf == true)
        {
            GM.AddPlayer(new Player("AI4", PlayerColor.YELLOW, new Color(1, (float)0.92, (float)0.016, 1), PlayerType.AI));
        }
        if (BlackAI.activeSelf == true)
        {
            GM.AddPlayer(new Player("AI5", PlayerColor.BLACK, new Color(0, 0, 0, 1), PlayerType.AI));
        }
        GM.Client_number = Client_number;
        GM.Connections = Connections;
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
    public void GoToLAN()
    {
        //GM.ShowAdPlacement();
        GM.SetGameState(GameState.LOBBY);
        Debug.Log(GM.gameState);
    }
    public void GoToAddPlayerMenu()
    {
        //GM.ShowAdPlacement();
        GM.SetGameState(GameState.ADD_PLAYER_MENU);
        Debug.Log(GM.gameState);
    }
    public void GoToSettings()
    {
        //GM.ShowAdPlacement();
    }
    private void HandlePlayerOptions(PlayerOptions options, ref GameObject frame, ref GameObject input, ref GameObject AI)
    {
        switch (options)
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
        if (Network.connections.Length == 0 || (Network.peerType == NetworkPeerType.Server))
        {
            playerOptionsRed++;
            if (playerOptionsRed == PlayerOptions.PLAYER_END)
            {
                playerOptionsRed = 0;
            }
            HandlePlayerOptions(playerOptionsRed, ref RedFrame, ref RedInput, ref RedAI);
            CheckStartGamePossibility();
        }
    }
    public void NextPlayerOptionGreen()
    {
        if (Network.connections.Length == 0 || Client_number == 1)
        {
            playerOptionsGreen++;
            if (playerOptionsGreen == PlayerOptions.PLAYER_END)
            {
                playerOptionsGreen = 0;
            }
            HandlePlayerOptions(playerOptionsGreen, ref GreenFrame, ref GreenInput, ref GreenAI);
            CheckStartGamePossibility();
        }
    }
    public void NextPlayerOptionBlue()
    {
        if (Network.connections.Length == 0 || Client_number == 2)
        {
            playerOptionsBlue++;
            if (playerOptionsBlue == PlayerOptions.PLAYER_END)
            {
                playerOptionsBlue = 0;
            }
            HandlePlayerOptions(playerOptionsBlue, ref BlueFrame, ref BlueInput, ref BlueAI);
            CheckStartGamePossibility();
        }
    }
    public void NextPlayerOptionYellow()
    {
        if (Network.connections.Length == 0 || Client_number == 3)
        {
            playerOptionsYellow++;
            if (playerOptionsYellow == PlayerOptions.PLAYER_END)
            {
                playerOptionsYellow = 0;
            }
            HandlePlayerOptions(playerOptionsYellow, ref YellowFrame, ref YellowInput, ref YellowAI);
            CheckStartGamePossibility();
        }
    }
    public void NextPlayerOptionBlack()
    {
        if (Network.connections.Length == 0 || Client_number == 4)
        {
            playerOptionsBlack++;
            if (playerOptionsBlack == PlayerOptions.PLAYER_END)
            {
                playerOptionsBlack = 0;
            }
            HandlePlayerOptions(playerOptionsBlack, ref BlackFrame, ref BlackInput, ref BlackAI);
            CheckStartGamePossibility();
        }
    }
    void Update()
    {
        int help = 0;
        NetworkMessage netMsg = new NetworkMessage();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            networkView.RPC("debug", RPCMode.All);
        }
        if (Network.peerType == NetworkPeerType.Server)
        {
            if (Network.connections.Length != Connenctions_number)
            {
                if (Network.connections.Length > Connenctions_number)
                {
                    for (int i = 0; i < 4; i++)
                        if (Connections[i] == false)
                        {
                            Connections[i] = true;
                            help = i + 1;
                            i = 10;
                        }
                    Connenctions_number += 1;
                    networkView.RPC("Client_PlayerJoined", RPCMode.All, help);
                    Debug.Log("player connect:" + Connenctions_number);
                    change = true;
                }
                else
                {
                    wait = Network.connections.Length;
                    for (int i = 0; i < 4; i++)
                    {
                        Connections[i] = false;
                        networkView.RPC("Client_PlayerDisconnet", RPCMode.All, i + 1);
                    }
                    Connenctions_number -= 1;
                    Debug.Log("player disconnect:" + Connenctions_number);
                    change = true;
                }
            }
            if (GM.gameState == GameState.LOBBY)
                if (Network.peerType == NetworkPeerType.Server && change == true && wait == 0)
                {
                    change = false;
                    networkView.RPC("Lobby", RPCMode.All, Connections);
                }
        }
        if (Network.peerType == NetworkPeerType.Client)
        {
        }
    }
    public void HandleOnStateChange()
    {
        Debug.Log("CHANGED STATE!");
    }
    void Start()
    {
        wait = 0;
        Connenctions_number = 0;
        player = 1;
        group = 1;
        Client_number = 0;
        Connections = new bool[4];
        for (int i = 0; i < 4; i++)
            Connections[i] = false;
        networkView = GetComponent<NetworkView>();
        if (networkView == null)
            Debug.Log("null");
    }
    void RpcFunction()
    {
        Debug.Log("esc");
    }
    [RPC]
    public void Client_PlayerJoined(int number)
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            if (Client_number == 0)
                Client_number = number;
            RedFrame.SetActive(true);
            RedInput.SetActive(true);
        }

    }
    [RPC]
    public void Client_PlayerDisconnet(int number)
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            if (Client_number == number)
                networkView.RPC("Check_Connection", RPCMode.All, number);
        }
    }
    [RPC]
    public void Check_Connection(int number)
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            Connections[number - 1] = true;
            wait--;
        }
    }
    [RPC]
    public void debug()
    {
        int z;
        if (Network.peerType == NetworkPeerType.Server)
            for (int i = 0; i < 4; i++)
            {
                z = i + 1;
                Debug.Log("gracz " + z + ":" + Connections[i]);
            }
    }
    [RPC]
    public void Lobby(bool[] conection)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Network.peerType == NetworkPeerType.Client && i != 0)
            {
                Connections[i - 1] = conection[i - 1];
                Debug.Log("Connections[" + i + "]:" + Connections[i]);
            }
            switch (i)
            {
                case 0:
                    RedFrame.SetActive(true);
                    RedInput.SetActive(true);
                    break;
                case 1:
                    if (conection[i - 1])
                    {

                        GreenFrame.SetActive(true);
                        GreenInput.SetActive(true);
                    }
                    else
                    {
                        GreenFrame.SetActive(false);
                        GreenInput.SetActive(false);
                    }
                    break;
                case 2:
                    if (conection[i - 1])
                    {
                        BlueFrame.SetActive(true);
                        BlueInput.SetActive(true);
                    }
                    else
                    {
                        BlueFrame.SetActive(false);
                        BlueInput.SetActive(false);
                    }
                    break;
                case 3:
                    if (conection[i - 1])
                    {
                        YellowFrame.SetActive(true);
                        YellowInput.SetActive(true);
                    }
                    else
                    {
                        YellowFrame.SetActive(false);
                        YellowInput.SetActive(false);
                    }
                    break;
                case 4:
                    if (conection[i - 1])
                    {
                        BlackFrame.SetActive(true);
                        BlackInput.SetActive(true);
                    }
                    else
                    {
                        BlackFrame.SetActive(false);
                        BlackInput.SetActive(false);
                    }
                    break;
            }
        }
    }
    [RPC]
    public void LanStartGame()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            StartGame();
        }
    }
}

