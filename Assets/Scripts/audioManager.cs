using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] AudioSource noiseSource;
    [SerializeField] float noiseVolume;
    [SerializeField] float audioVolume;
    public AudioSource audiosource;
    public AudioClip[] clips;
    public AudioClip[] currentPlaylist;
    public bool canPlay;

    public enum soundTypes {
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
            if (!audiosource.isPlaying && currentPlaylist.Length > 0 )
            {
                audiosource.PlayOneShot(currentPlaylist[0]);
                currentPlaylist = currentPlaylist.Skip(1).ToArray();

            }
            audioVolume = audioVolume + (0.02f * Time.deltaTime);
        }
        else
        {
            audioVolume = audioVolume - (0.02f * Time.deltaTime);
            if (currentPlaylist.Length > 0 || audiosource.isPlaying)
            {
                playNoise = true;
            }
        }
        if (playNoise)
        {
            noiseVolume = noiseVolume + (0.02f*Time.deltaTime);
        }
        else
        {
            noiseVolume = noiseVolume - (0.02f*Time.deltaTime);
        }
        noiseVolume = Mathf.Clamp(noiseVolume, 0f, 1f);
        audioVolume = Mathf.Clamp(audioVolume, 0f, 1f);
        noiseSource.volume = noiseVolume;
        audiosource.volume = audioVolume;
    }

    public void addAudio(string path)
    {
        string p = Path.Combine(Application.dataPath,"audio/"+path);
        WWW clipPath = new WWW(p);
        print(clipPath);  //It displays a correct path but puts a back slash \ before the Audio folder.
                          //When I put a forward slash / before the Audio folder in the string, print only returns the path from the Audio folder onward.
        AudioClip audioClip = clipPath.GetAudioClip();

        currentPlaylist.Append(audioClip);
    }



}
