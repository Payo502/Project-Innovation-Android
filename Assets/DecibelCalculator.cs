using System.Collections;
using UnityEngine;
public class DecibelCalculator : MonoBehaviour
{
    public static float MicLoudness;

    [SerializeField] float micSensitivity;
    [SerializeField] float cooldownTime = 1f;

    private string device;

    private bool screamDetected = false;

    void InitMic()
    {
        if (device == null) device = Microphone.devices[0];
        _clipRecord = Microphone.Start(device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(device);
    }


    AudioClip _clipRecord = null;
    int _sampleWindow = 128;

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void Update()
    {
        MicCalculation();
    }

    void MicCalculation()
    {
        if (!screamDetected)
        {
            MicLoudness = LevelMax();

            if (MicLoudness >= micSensitivity)
            {
                Debug.Log($"Scream Detected: {MicLoudness}");
                screamDetected = true;

                StartCoroutine(ScreamCooldown());

                // Change this to whatever we want to send
                ClientMessageManager.Singleton.SendStringMessagesToServer(ClientToServerId.stringMessage, "I AM SCREAMING!!");
            }
        }
    }

    IEnumerator ScreamCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        screamDetected = false;
    }

    bool _isInitialized;
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {

            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;

        }
    }
}
