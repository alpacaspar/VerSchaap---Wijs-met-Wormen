using System;
using System.Net;
using System.Threading;
using UnityEngine;

public class HttpListenerTest : MonoBehaviour
{
    private HttpListener listener;
    private Thread httpListenerThread;

    private void Start()
    {
        ConnectToHttpServer();
    }
    
    private void ConnectToHttpServer()
    {
        try
        {
            httpListenerThread = new Thread(CreateListener)
            {
                IsBackground = true
            };
            httpListenerThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnApplicationQuit()
    {
        listener.Close();
    }

    private void CreateListener()
    {
        HttpListenerContext context = null;
        HttpListenerRequest request = null;
        HttpListenerResponse response = null;
        
        string port = "9876";
        string requestUrl;

        bool listen;

        try
        {
            if (listener == null)
            {
                listener = new HttpListener();
                listener.Prefixes.Add($"http://*:{port}/");
                listener.Start();
                Debug.Log("Http thing is listening...");
                listen = true;
                while (listen)
                {
                    try
                    {
                        context = listener.GetContext();
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        listen = false;
                    }

                    if (listen)
                    {
                        request = context.Request;
                        requestUrl = request.Url.ToString();

                        response = context.Response;
                        string responseString = "Hello";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer,0,buffer.Length);

                        output.Close();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
