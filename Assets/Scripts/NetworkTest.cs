using System.Net;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    private void Start()
    {
        if (Application.isEditor)
        {
            transform.AddComponent<HttpListenerTest>();
        }
        else
        {
            transform.AddComponent<HttpClientTest>();
        }
    }
    
    public static string GetLocalIP()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }

        }
        return localIP;
    }
}
