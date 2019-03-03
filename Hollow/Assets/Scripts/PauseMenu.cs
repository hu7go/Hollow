using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    GameController gameController;
    EventSystem es;
    public UnityEvent resumeEvent;

    public Canvas pauseMenu;
    public PlayerController player;
    public Image hintImage;

    public Button setButton01;
    public Button setButton02;

    public GameObject pauseObj;
    public GameObject optionsObj;

    private bool paused = false;

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
        es = FindObjectOfType<EventSystem>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu"))
        {
            if (!paused)
            {
                Paused();

                es.SetSelectedGameObject(setButton01.gameObject);
            }
            else
            {
                UnPaused();
            }
        }
    }

    public void ToggleHints()
    {
        GameManager.Instance.ToggleHints(hintImage, gameController.hideHints);

        //hintImage.enabled = true;
        gameController.hideHints = !gameController.hideHints;
    }

    public void Paused ()
    {
        pauseMenu.gameObject.SetActive(true);
        paused = true;
        gameController.Pause();
        gameController.isPaused = true;

        pauseObj.SetActive(true);
        SetPauseButton();
    }

    public void UnPaused ()
    {
        resumeEvent.Invoke();
        paused = false;
        pauseMenu.gameObject.SetActive(false);
        gameController.UnPause();
        gameController.isPaused = false;

        pauseObj.SetActive(false);
    }

    public void ExitToMaintMenu ()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ExitToMain();
        else
        {
            Debug.Log("There is currently no game manager in this scene!");
        }
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SetButton ()
    {
        es.SetSelectedGameObject(setButton02.gameObject);
    }

    public void SetPauseButton ()
    {
        es.SetSelectedGameObject(setButton01.gameObject);
    }
}
