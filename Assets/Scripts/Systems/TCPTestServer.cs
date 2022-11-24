using System;
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading; 
using UnityEngine;

public class TCPTestServer : MonoBehaviour
{
	private TcpListener tcpListener;
	private Thread tcpListenerThread;
	private TcpClient connectedTcpClient;

	private void Start()
	{
		tcpListenerThread = new Thread(ListenForIncommingRequests)
		{
			IsBackground = true
		};
		tcpListenerThread.Start();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("sent message");
			SendMessage();
		}
	}

	private void OnApplicationQuit()
	{
		tcpListener.Stop();
	}

	private void ListenForIncommingRequests()
	{
		try
		{
			Debug.Log("Host name: " + Dns.GetHostName());
			IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress ipAddress = ipHost.AddressList[0];
			tcpListener = new TcpListener(ipAddress, 8052);
			tcpListener.Start();
			Debug.Log("Server is listening");
			byte[] bytes = new byte[1024];
			while (true)
			{
				if (connectedTcpClient == null) return;
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							byte[] incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							string clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.Log("client message received as: " + clientMessage);
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException);
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
}