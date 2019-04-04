using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer master;

    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void SetMasterLevel(float newVolume)
    {
        masterVolume = newVolume;
        master.SetFloat("MasterVolume", newVolume);
    }

    public void SetMusicLevel(float newVolume)
    {
        musicVolume = newVolume;
        master.SetFloat("MusicVolume", newVolume);
    }

    public void SetEffectsLevel(float newVolume)
    {
        effectsVolume = newVolume;
        master.SetFloat("EffectsVolume", newVolume);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetEffectsVolume()
    {
        return effectsVolume;
    }
}
