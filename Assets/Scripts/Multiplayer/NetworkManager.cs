using Riptide;
using Riptide.Utils;
using System;
using System.Net;
using TMPro;
using UnityEngine;


public enum ClientToServerId : ushort
{
    stringMessage = 1,
    floatMessage = 2,
    intMessage = 3,
    boolMessage = 4,
}

public enum ServerToClientId : ushort
{
    stringMessage = 1,
    floatMessage = 2,
    intMessage = 3,
    boolMessage = 4,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;


    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;


    void logerror() { 
    }
    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
/*        IPHostEntry host = Dns.GetHostEntry(ip);
        Client.Connect($"{host.AddressList[0]}:{port}");*/

        Client.Connect($"{ip}:{port}");

    }

    private void DidConnect(object sender, EventArgs e)
    {
        //UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    public void ChangeIp(string str)
    {
        ip = str;
    }
}
