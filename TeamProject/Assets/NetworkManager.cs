using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(GetLocalIPv4(NetworkInterfaceType.Ethernet).FirstOrDefault());
    }

    internal static string GetLocalIPv4(NetworkInterfaceType _type)
    {
        string output = "";
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties adapterProperties = item.GetIPProperties();

                if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                {
                    foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return output;
    }

    // Update is called once per frame
    void Update () {
	
	}
}

