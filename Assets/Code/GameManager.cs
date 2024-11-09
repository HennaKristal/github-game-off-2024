using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ??= FindFirstObjectByType<GameManager>();
    private InputController inputController;
    private Fading fading;

    [Header("Parts")]
    public List<PlaneStats> allPlaneCoreParts;
    public List<EngineStats> allEngineParts;
    public List<GeneratorStats> allGeneratorParts;
    public List<CoolerStats> allCoolerParts;
    public List<TokenStats> allTokens;
    public List<BadgeStats> allBadges;
    public List<WeaponStats> allMainWeaponsParts;
    public List<WeaponStats> allLeftInnerWeaponParts;
    public List<WeaponStats> allLeftOuterWeaponParts;
    public List<WeaponStats> allRightInnerWeaponParts;
    public List<WeaponStats> allRightOuterWeaponParts;

    [Header("Missions")]
    public MissionStats[] missions;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        inputController = GetComponent<InputController>();
        fading = GetComponent<Fading>();
    }

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

    public InputController GetInputController()
    {
        return inputController;
    }

    public void LoadSceneByName(string sceneName) => StartCoroutine(ChangeScene(sceneName));
    private IEnumerator ChangeScene(string sceneName)
    {
        fading.StartFadeOut(2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
