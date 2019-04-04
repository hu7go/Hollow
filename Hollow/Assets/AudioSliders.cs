using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    private Slider slider;

    private float value;

    public AudioVersion audioVersion;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        value = slider.value;
        switch (audioVersion)
        {
            case AudioVersion.master:
                SoundManager.Instance.SetMasterLevel(value);
                break;
            case AudioVersion.music:
                SoundManager.Instance.SetMusicLevel(value);
                break;
            case AudioVersion.effects:
                SoundManager.Instance.SetEffectsLevel(value);
                break;
        }
    }
}

public enum AudioVersion
{
    master,
    music,
    effects
}
