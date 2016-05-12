using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

    public string localIP;
    public GameObject Testtext;
    private Text IP;


	// Use this for initialization
	void Start () {
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("192.168.0.77", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        Debug.Log(localIP);
        Testtext.GetComponent<Text>().text = localIP;
    }


    // Update is called once per frame
    void Update () {
	
	}
}

