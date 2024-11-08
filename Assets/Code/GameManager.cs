using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Fading fading;

    public void Start()
    {
        fading.StartFadeIn(2f);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }

    public void LoadSceneByName(string sceneName) => StartCoroutine(ChangeScene(sceneName));
    private IEnumerator ChangeScene(string sceneName)
    {
        fading.StartFadeOut(2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
