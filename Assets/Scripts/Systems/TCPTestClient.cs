using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPTestClient : MonoBehaviour
{
	private TcpClient socketConnection;
	private Thread clientReceiveThread;

	private void Start()
	{
		ConnectToTcpServer();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SendMessage();
		}
	}

	private void OnApplicationQuit()
	{
		socketConnection?.Close();
	}

	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(ListenForData)
			{
				IsBackground = true
			};
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	
	private void ListenForData()
	{
		try
		{
			IPAddress ipAddress = IPAddress.Parse("145.107.199.232");
			socketConnection = new TcpClient();
			socketConnection.Connect(ipAddress, 8052);
			byte[] bytes = new byte[1024];
			
			while (true)
			{
				if (socketConnection == null) return;
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						string serverMessage = Encoding.ASCII.GetString(incommingData);
						Debug.Log("server message received as: " + serverMessage);
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
	
	private void SendMessage()
	{
		if (socketConnection == null) return;
		
		try
		{
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				string clientMessage = "This is a message from one of your clients.";
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log("Client sent his message - should be received by server");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}