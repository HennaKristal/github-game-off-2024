using UnityEngine;

public class MissionController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    public MissionStats missionData;

    private void Start()
    {
        foreach (MissionStats mission in GameManager.Instance.missions)
        {
            if (mission.sceneName == playerStats.playingNow)
            {
                missionData = mission;
                break;
            }
        }

        if (missionData == null)
        {
            Debug.LogError("Failed to load mission data");
            GameManager.Instance.LoadSceneByName("Garage");
        }
    }

    public void CompleteMission(int score)
    {
        // TODO: Show UI with results
        GameManager.Instance.LoadSceneByName("Garage");

        playerStats.progressStep = missionData.advenceToStep;
    }

    public void FailMission(int score)
    {
        // TODO: Show UI with results
        GameManager.Instance.LoadSceneByName("Garage");
        playerStats.progressStep = missionData.advenceToStep;
    }

    public void GameOver()
    {
        GameManager.Instance.LoadSceneByName("Main Menu");
    }

    public void AbortMission()
    {
        GameManager.Instance.LoadSceneByName("Garage");
    }

    public void RestartMission()
    {
        GameManager.Instance.LoadSceneByName(missionData.sceneName);
    }
}
