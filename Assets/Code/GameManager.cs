using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetTargetFramerate(60);
    }

    private void SetTargetFramerate(int fps = 60)
    {
        Application.targetFrameRate = fps;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}
