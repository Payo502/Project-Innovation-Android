using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;


[Serializable]
public struct NamedAudioClip
{
    public string name;
    public AudioClip clip;
    public bool instant;
}

public class audioManager : MonoBehaviour
{
    [SerializeField] AudioSource noiseSource;
    [SerializeField] float noiseVolume;
    [SerializeField] float audioVolume;
    public AudioSource audiosource;
    [SerializeField]
    private List<NamedAudioClip> namedAudioClips = new List<NamedAudioClip>();
    public List<AudioClip> currentPlaylist = new List<AudioClip>();
    public bool canPlay;
    public string lastPlayedClip = "";

    public enum soundTypes
    {
    }

    void Start()
    {
        noiseVolume = 0;
        audioVolume = 0;
    }

    void Update()
    {
        bool playNoise = false;
        if (canPlay)
        {
            if (!audiosource.isPlaying && lastPlayedClip != "") { GameObject.Find("NetworkManager").GetComponent<ClientMessageManager>().SendStringMessagesToServer(ClientToServerId.stringMessage, lastPlayedClip); lastPlayedClip = ""; }
            if (!audiosource.isPlaying && currentPlaylist.Count > 0)
            {
                audiosource.PlayOneShot(currentPlaylist[0]);
                currentPlaylist.RemoveAt(0);
                if (lastPlayedClip != "") { }
            }
            audioVolume = audioVolume + (0.4f * Time.deltaTime);
        }
        else
        {
            audioVolume = audioVolume - (0.4f * Time.deltaTime);
            if (currentPlaylist.Count > 0 || audiosource.isPlaying)
            {
                playNoise = true;
            }
        }
        if (playNoise)
        {
            noiseVolume = noiseVolume + (0.4f*Time.deltaTime);
        }
        else
        {
            noiseVolume = noiseVolume - (0.4f*Time.deltaTime);
        }
        noiseVolume = Mathf.Clamp(noiseVolume, 0f, 1f);
        audioVolume = Mathf.Clamp(audioVolume, 0f, 1f);
        noiseSource.volume = noiseVolume;
        audiosource.volume = audioVolume;
    }

    public void addAudio(string name)
    {
        var namedClip = namedAudioClips.FirstOrDefault(nac => nac.name == name);


        if (namedClip.clip != null)
        {
            if (namedClip.instant)
            {
                audiosource.PlayOneShot(currentPlaylist[0]);
            }
            else
            {
                Debug.Log("Adding clip: " + namedClip.name);
                currentPlaylist.Add(namedClip.clip);
                Debug.Log(currentPlaylist.Count);
                lastPlayedClip = namedClip.name;
            }
        }
        else
        {
            Debug.LogError("Failed to find clip with name: " + name);
        }
    }
}
