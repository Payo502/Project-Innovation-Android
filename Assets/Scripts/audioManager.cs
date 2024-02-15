using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] AudioSource noiseSource;
    [SerializeField] float noiseVolume;
    [SerializeField] float audioVolume;
    public AudioSource audiosource;
    public List<AudioClip> currentPlaylist = new List<AudioClip>();
    public bool canPlay;

    public enum soundTypes
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        noiseVolume = 0;
        audioVolume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool playNoise = false;
        if (canPlay)
        {
            if (!audiosource.isPlaying && currentPlaylist.Count > 0)
            {
                audiosource.PlayOneShot(currentPlaylist[0]);
                currentPlaylist.RemoveAt(0); // Remove the first element after playing it
            }
            audioVolume = audioVolume + (0.1f * Time.deltaTime);
        }
        else
        {
            audioVolume = audioVolume - (0.1f * Time.deltaTime);
            if (currentPlaylist.Count > 0 || audiosource.isPlaying)
            {
                playNoise = true;
            }
        }
        if (playNoise)
        {
            noiseVolume = noiseVolume + (0.1f*Time.deltaTime);
        }
        else
        {
            noiseVolume = noiseVolume - (0.1f*Time.deltaTime);
        }
        noiseVolume = Mathf.Clamp(noiseVolume, 0f, 1f);
        audioVolume = Mathf.Clamp(audioVolume, 0f, 1f);
        noiseSource.volume = noiseVolume;
        audiosource.volume = audioVolume;
    }

    public void addAudio(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>("Assets/audio/" + path + ".wav");
        if (clip != null)
        {
            Debug.Log(clip);
            currentPlaylist.Add(clip);
        }
        else
        {
            Debug.LogError("Failed to load clip at path: " + path);
        }
    }



}
