using Riptide;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;

    [Header("Received Message")]
    [SerializeField] private GameObject sendMessageUI;
    [SerializeField] private TextMeshProUGUI messageToDisplay;

    [Header("Test Message To Send")]
    [SerializeField] private string stringMessage;
    [SerializeField] private int intMessage;
    [SerializeField] private float floatMessage;
    [SerializeField] private bool boolMessage;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        connectUI.SetActive(false);
        sendMessageUI.SetActive(true);

        NetworkManager.Singleton.Connect();
    }

    public void BackToMain()
    {
        connectUI.SetActive(true);
        sendMessageUI.SetActive(false);
    }

    public void StringMessageClicked()
    {
        ClientMessageManager.Singleton.SendStringMessagesToServer(ClientToServerId.stringMessage, stringMessage);
    }
    public void IntMessageClicked()
    {
        ClientMessageManager.Singleton.SendIntMessagesToServer(ClientToServerId.intMessage, intMessage);
    }
    public void FloatMessageClicked()
    {
        ClientMessageManager.Singleton.SendFloatMessagesToServer(ClientToServerId.floatMessage, floatMessage);
    }

    public void DisplayMessage(object content)
    {
        if (messageToDisplay != null)
        {
            messageToDisplay.text = content.ToString();
        }
        else
        {
            Debug.LogWarning("Message display component not set in UIManager");
        }
    }
}
