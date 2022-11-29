using System;
using System.Net; 
using System.Net.Sockets; 
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPTestServer : MonoBehaviour
{
    private Thread socketThread;
    private TcpClient connectedTcpClient;
    volatile bool keepReading = false;

// Use this for initialization
    private void Start()
    {
        Application.runInBackground = true;
        StartServer();
    }

    private void StartServer()
    {
        socketThread = new Thread(NetworkCode)
        {
            IsBackground = true
        };
        socketThread.Start();
    }

    private Socket listener;
    private Socket handler;

    private void NetworkCode()
    {
        string data;

        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // host running the application.
        Debug.Log("Ip " + NetworkTest.GetLocalIP());
        IPAddress[] ipArray = Dns.GetHostAddresses(NetworkTest.GetLocalIP());
        IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 1755);

        // Create a TCP/IP socket.
        listener = new Socket(ipArray[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.
            while (true)
            {
                keepReading = true;

                // Program is suspended while waiting for an incoming connection.
                Debug.Log("Waiting for Connection"); //It works

                handler = listener.Accept();
                Debug.Log("Client Connected"); //It doesn't work
                data = null;

                // An incoming connection needs to be processed.
                while (keepReading)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    Debug.Log("Received from Server");

                    if (bytesRec <= 0)
                    {
                        keepReading = false;
                        handler.Disconnect(true);
                        Debug.Log("Disconnecting socket");
                        break;
                    }

                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                    Thread.Sleep(1);
                }

                Thread.Sleep(1);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    
    private void SendMessage()
    {
        if (connectedTcpClient == null) return;

        try
        {
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = "This is a message from your server.";
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);

                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    private void StopServer()
    {
        keepReading = false;
        handler?.Close();
        listener?.Close();
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }
}