using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool hideHints = false;
    public bool isPaused = false;
    public bool inInventory = false;

    PlayerController player;
    //CameraFollow mainCamera;


    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void Start()
    {
        //This finds the player and the camera,
        //but it will only work if there is one player will have to change if there is coop/multiplayer
        player = FindObjectOfType<PlayerController>();
        //mainCamera = FindObjectOfType<CameraFollow>();
    }

    public void Pause ()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        CameraFollow.Instance.GetComponent<CameraFollow>().enabled = false;
        //mainCamera.enabled = false;
    }

    public void UnPause ()
    {
        Time.timeScale = 1;
        player.enabled = true;
        CameraFollow.Instance.GetComponent<CameraFollow>().enabled = true;
        //mainCamera.enabled = true;
    }
}
