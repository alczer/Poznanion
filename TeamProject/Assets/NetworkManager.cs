using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetworkManager : MonoBehaviour {

    public string localIP;
    public GameObject Testtext;
    private Text IP;
    String text;
    bool newmessage = false;

    TcpListener tcpListener = new TcpListener(10);

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

        Server();
    }

    void Server()
    {
        tcpListener.Start();
        Debug.Log("************This is Server program************");
        Debug.Log("Hoe many clients are going to connect to this server?:");
        int numberOfClientsYouNeedToConnect = 5;

        for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
        {
            Thread newThread = new Thread(new ThreadStart(Listeners));
            newThread.Start();
        }
    }

    void Listeners()
    {
        Socket socketForClient = tcpListener.AcceptSocket();
        //new TcpListener(10)
        if (socketForClient.Connected)
        {
            Debug.Log("Client:" + socketForClient.RemoteEndPoint + " now connected to server.");
            
            //text = "\nClient:" + socketForClient.RemoteEndPoint + " now connected to server.";
            //newmessage = true;
            
            NetworkStream networkStream = new NetworkStream(socketForClient);
            System.IO.StreamWriter streamWriter =
            new System.IO.StreamWriter(networkStream);
            System.IO.StreamReader streamReader =
            new System.IO.StreamReader(networkStream);

            ////here we send message to client
            //Debug.Log("type your message to be recieved by client:");
            //string theString = Console.ReadLine();
            //streamWriter.WriteLine(theString);
            ////Debug.Log(theString);
            //streamWriter.Flush();

            //while (true)
            //{
            //here we recieve client's text if any.
            while (true)
            {
                string theString = streamReader.ReadLine();
                Debug.Log("Message recieved by client:" + theString);
                
                //text = "Message recieved by client:" + theString;
                //newmessage = true;
                
                if (theString == "exit")
                    break;
            }
            streamReader.Close();
            networkStream.Close();
            streamWriter.Close();
            //}

        }
        socketForClient.Close();
    }

    public void Client()
    {
        String addr = "192.168.0.18";

        TcpClient socketForServer;
        try
        {
            socketForServer = new TcpClient(addr, 10);
        }
        catch
        {
            Debug.Log("Failed to connect to server at " + addr);
            return;
        }

        NetworkStream networkStream = socketForServer.GetStream();
        System.IO.StreamReader streamReader =
        new System.IO.StreamReader(networkStream);
        System.IO.StreamWriter streamWriter =
        new System.IO.StreamWriter(networkStream);
        Debug.Log("*******This is client program who is connected to localhost on port No:10*****");

        try
        {
            string outputString;
            // read the data from the host and display it
            {
                //outputString = streamReader.ReadLine();
                //Debug.Log("Message Recieved by server:" + outputString);

                //Debug.Log("Type your message to be recieved by server:");
                //Debug.Log("type:");


                //string str = "x";
                //    //Console.ReadLine();

                //while (str != "exit")
                //{
                //    streamWriter.WriteLine(str);
                //    streamWriter.Flush();
                //    Debug.Log("type:");
                //    str = Console.ReadLine();
                //}
                //if (str == "exit")
                //{
                //    streamWriter.WriteLine(str);
                //    streamWriter.Flush();
                //}

            }
        }
        catch
        {
            Debug.Log("Exception reading from Server");
        }
        // tidy up
        networkStream.Close();
        Debug.Log("Press any key to exit from client program");
    }



    // Update is called once per frame
    void Update () {
        //if (newmessage)
        //{
        //    Testtext.GetComponent<Text>().text = text;
        //    newmessage = false;
        //}
	}
}

