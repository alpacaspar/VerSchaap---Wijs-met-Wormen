using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Threading;

public class HttpListenerTest : MonoBehaviour
{
    private HttpListener listener;
    private Thread listenerThread;

    private void Start()
    {
        StartServer($"http://{NetworkTest.GetLocalIP()}:{8052}/");
    }

    /// <summary>
    /// Starts a server with the given URI Address.
    /// </summary>
    /// <param name="uri">Uri string</param>
    public void StartServer(string uri)
    {
        listener = new HttpListener();

        listener.Prefixes.Add("http://localhost:4444/");
        listener.Prefixes.Add("http://127.0.0.1:4444/");
        listener.Prefixes.Add(uri);

        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();

        listenerThread = new Thread(StartListener);
        listenerThread.Start();

        Debug.Log("Server Started");
    }

    private void StartListener()
    {
        while (true)
        {
            try
            {
                IAsyncResult result = listener.BeginGetContext(ListenerCallback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        HttpListenerContext context = listener.EndGetContext(result);

        Debug.Log("Method: " + context.Request.HttpMethod);
        Debug.Log("LocalUrl: " + context.Request.Url.LocalPath);

        if (context.Request.QueryString.AllKeys.Length > 0)
        {
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                Debug.Log("Key: " + key + ", Value: " + context.Request.QueryString.GetValues(key)?[0]);
            }
        }

        if (context.Request.HttpMethod == "POST")
        {
            Thread.Sleep(1000);
            string data = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            WeatherInfo json = JsonUtility.FromJson<WeatherInfo>(data);
            Debug.Log(json.timezone);
            EventSystem<WeatherInfo>.InvokeEvent(EventType.weatherDataReceived, json);
        }

        context.Response.Close();
    }
}