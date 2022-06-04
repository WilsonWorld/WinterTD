using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer TheMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol")) {
            TheMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            TheMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            TheMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        }
    }
}
