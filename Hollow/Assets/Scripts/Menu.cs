using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject options;

    [Space(20)]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    public void PauseActive ()
    {
        pause.SetActive(true);
        options.SetActive(false);
    }

    public void OptionsActive ()
    {
        pause.SetActive(false);
        options.SetActive(true);

        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        musicSlider.value = SoundManager.Instance.GetMusicVolume();
        effectsSlider.value = SoundManager.Instance.GetEffectsVolume();
    }
}
