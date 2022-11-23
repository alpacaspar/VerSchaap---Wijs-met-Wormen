using System;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    private void Start()
    {
        if (Application.isEditor)
        {
            transform.AddComponent<TCPTestServer>();
        }
        else
        {
            transform.AddComponent<TCPTestClient>();
        }
    }
}
