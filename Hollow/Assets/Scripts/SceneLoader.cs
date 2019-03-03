using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool loadScene = false;

    [SerializeField] private int sceneNumber = 1;
    [SerializeField] private Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene ()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneNumber);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
