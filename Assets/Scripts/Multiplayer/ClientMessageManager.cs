using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine;

public class ClientMessageManager : MonoBehaviour
{
    private static ClientMessageManager _singleton;

    public static ClientMessageManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ClientMessageManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    #region HANDLE SENDING MESSAGES TO SERVER
    /// <summary>
    /// Handle sending messages to the server
    /// </summary>
    /// <param name="messageId">Id of the message</param>
    /// <param name="messageContent">Content of the message</param>
    public void SendStringMessagesToServer(ClientToServerId messageId, string messageContent)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)messageId);
        message.AddString(messageContent);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendIntMessagesToServer(ClientToServerId messageId, int messageContent)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)messageId);
        message.AddInt(messageContent);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendFloatMessagesToServer(ClientToServerId messageId, float messageContent)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)messageId);
        message.AddFloat(messageContent);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendBoolMessagesToServer(ClientToServerId messageId, bool messageContent)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)messageId);
        message.AddBool(messageContent);
        NetworkManager.Singleton.Client.Send(message);
    }
    #endregion

    #region HANDLE RECEIVING MESSAGES FROM SERVER
    /// <summary>
    /// Handle string messages received from server
    /// </summary>
    /// <param name="message">message received from server</param>
    [MessageHandler((ushort)ServerToClientId.stringMessage)]
    private static void OnStringMessageRecieved(Message message)
    {
        string content = message.GetString();
        Debug.Log($"{content} was received by the Server");
        UIManager.Singleton.DisplayMessage(content);
        GameObject.Find("AudioPlayer").GetComponent<audioManager>().addAudio(content);
    }

    [MessageHandler((ushort)ServerToClientId.intMessage)]
    private static void OnIntMessageReceive(Message message)
    {
        int content = message.GetInt();
        Debug.Log($"{content} was received by the Server");
        UIManager.Singleton.DisplayMessage(content);
    }

    [MessageHandler((ushort)ServerToClientId.floatMessage)]
    private static void OnFloatMessage(Message message)
    {
        float content = message.GetFloat();
        Debug.Log($"{content} was received by the Server");
        UIManager.Singleton.DisplayMessage(content);
    }

    [MessageHandler((ushort)ServerToClientId.boolMessage)]
    private static void OnStringMessage(Message message)
    {
        bool content = message.GetBool();
        Debug.Log($"{content} was received by the Server");
        UIManager.Singleton.DisplayMessage(content);
    }
    #endregion
}
