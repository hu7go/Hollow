using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameName;
    [SerializeField] private GameObject firstMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject pressAnyButton;
    [SerializeField] private Image hints;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private MenuAnimations menuAnims;

    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject playButton;

    [Space(20)]
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private GameObject howToPlayBackButton;

    [Space(20)]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    [Space(20)]
    [SerializeField] private Animator ui;
    [SerializeField] private Animator camAnim;

    [Space(20)]
    [SerializeField] private MenuPlayer playerAnim;
    [SerializeField] private Click click;

    private bool openMenu = false;
    private bool canOpenMenu = false;

    private bool toggle = true;

    EventSystem es;

    //For filming and presentation only
    [Space(20)]
    [SerializeField] private Animator blackAnim;
    [SerializeField] private Animator titleAnim;
    [SerializeField] private Animator titleAnim2;
    AudioSource aS;
    //

    void Start()
    {
        //This checks if there are any gamepads selected, if so they remove the cursor.
        if (Input.GetJoystickNames().Length > 0)
            Cursor.visible = false;

        es = FindObjectOfType<EventSystem>();
        aS = GetComponent<AudioSource>();

        StartCoroutine(Delay());
        StartCoroutine(MusicDelay());

        blackAnim.SetBool("Start", true);
        titleAnim.SetBool("Start", true);
        titleAnim2.SetBool("Start", true);
        camAnim.SetTrigger("StartMenu");
    }

    private IEnumerator MusicDelay ()
    {
        yield return new WaitForSeconds(13);
        ui.SetBool("StartAnim", true);
    }

    private IEnumerator Delay ()
    {
        yield return new WaitForSeconds(6);
        //Starts the animations happening in the menu
        menuAnims.StartAnims();

        pressAnyButton.SetActive(true);
        canOpenMenu = true;
    }

    void Update()
    {
        if (Input.anyKey && !openMenu && canOpenMenu)
        {
            click.Clicking();

            openMenu = true;
            FirstMenu();
        }
    }

    public void StartGame ()
    {
        GameManager.Instance.StartMusic();
        SceneManager.LoadScene(gameName);
    }

    public void HowToPlay()
    {
        playMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        es.SetSelectedGameObject(howToPlayBackButton);
    }

    private void FirstMenu ()
    {
        firstMenu.SetActive(true);
        pressAnyButton.SetActive(false);
        es.SetSelectedGameObject(firstButton);
    }

    public void OpenOptions ()
    {
        options.SetActive(true);
        firstMenu.SetActive(false);
        es.SetSelectedGameObject(optionsButton);

        masterSlider.value = GameManager.Instance.GetMasterVolume();
        musicSlider.value = GameManager.Instance.GetMusicVolume();
        effectsSlider.value = GameManager.Instance.GetEffectsVolume();
    }

    public void PlayMenu ()
    {
        camAnim.SetBool("Start", true);
        camAnim.GetComponent<MouseFollower>().StartActive();
        playerAnim.Move();

        playMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
        firstMenu.SetActive(false);
        es.SetSelectedGameObject(playButton);
    }

    public void PlayBack ()
    {
        camAnim.SetBool("Start", false);
        camAnim.GetComponent<MouseFollower>().StartDeactive();
        playerAnim.MoveBack();

        playMenu.SetActive(false);
        firstMenu.SetActive(true);
        es.SetSelectedGameObject(firstButton);
    }

    public void CloseOptions ()
    {
        es.SetSelectedGameObject(firstButton);
        options.SetActive(false);
        firstMenu.SetActive(true);
    }

    public void ToggleHints()
    {
        GameManager.Instance.ToggleHints(hints, toggle);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
