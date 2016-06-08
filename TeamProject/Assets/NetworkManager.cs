using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;
using System.Threading;
using UnityEngine.Networking.Match;

public class NetworkManager : NetworkBehaviour
{
    public string IP = "127.0.0.1";
    public int Port = 25001;
    public GameObject testtext;
    private HostData[] hostList;
    void Awake()
    {

    }
    void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            GUI.Label(new Rect(100, 100, 100, 25), "Server");
            GUI.Label(new Rect(100, 125, 100, 25), "Connections: " + Network.connections.Length);
            if (GUI.Button(new Rect(100, 150, 100, 25), "Logout"))
            {
                Network.Disconnect(250);
            }
        }
        else
            if (Network.peerType == NetworkPeerType.Client)
            {
                GUI.Label(new Rect(100, 100, 100, 25), "Client");
                GUI.Label(new Rect(100, 125, 100, 25), "Connections: " + Network.connections.Length);
                if (GUI.Button(new Rect(100, 150, 100, 25), "Logout"))
                {
                    Network.Disconnect(250);
                }

            }
        if (GUI.Button(new Rect(100, 175, 100, 25), "rpc"))
        {
            Debug.Log("nadus esc");
        }
        if (hostList != null)
        {
            for (int i = 0; i < hostList.Length; i++)
            {
                if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                    JoinServer(hostList[i]);
            }
        }
    }
    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }
    void Update()
    {

    }
    void Start()
    {
    }
    private void RefreshHostList()
    {

        MasterServer.RequestHostList("typeName");
    }
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }
    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }
    public void client_start()
    {
        RefreshHostList();
    }
    public void serwer_start()
    {
        Network.InitializeServer(10, Port, !Network.HavePublicAddress());
        MasterServer.RegisterHost("typeName", "gameName");
    }
    void sprawdzam(NetworkMessage msg)
    {
        Debug.Log("sprawdzam");

    }
    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
    public class PlayerMessage : MessageBase
    {
        public string name;
        public int id;
    }
}



