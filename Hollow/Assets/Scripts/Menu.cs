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

        masterSlider.value = GameManager.Instance.GetMasterVolume();
        musicSlider.value = GameManager.Instance.GetMusicVolume();
        effectsSlider.value = GameManager.Instance.GetEffectsVolume();
    }
}
