using System;
using System.Net;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    private void Start()
    {
        if (!Application.isEditor)
        {
            transform.AddComponent<HttpListenerTest>();
        }
        else
        {
            transform.AddComponent<HttpClientTest>();
        }
    }
    
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
