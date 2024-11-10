using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private InputController inputController;
    public MissionStats missionData;
    private Dictionary<string, int> bonuses = new Dictionary<string, int>();
    private Dictionary<string, int> penalties = new Dictionary<string, int>();
    private float ammoCost;
    private float repairCost;
    private bool missionFinished = false;

    [Header("UI")]
    [SerializeField] private GameObject endResultView;
    [SerializeField] private TextMeshProUGUI endResultTitle;
    [SerializeField] private TextMeshProUGUI paymentText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private TextMeshProUGUI repairText;
    [SerializeField] private TextMeshProUGUI ammoCostText;
    [SerializeField] private TextMeshProUGUI PenaltyText;

    [Header("Score")]
    [SerializeField] private Image scoreImage;
    [SerializeField] private Sprite[] scoreStarSprites;

    private void Start()
    {
        inputController = GameManager.Instance.GetInputController();

        foreach (MissionStats mission in GameManager.Instance.missions)
        {
            if (mission.sceneName == playerStats.selectedLevel)
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

    private void Update()
    {
        if (missionFinished)
        {
            if (inputController.dodgePressed)
            {
                GameManager.Instance.SaveData();
                GameManager.Instance.LoadSceneByName("Garage");
            }
        }
    }

    public void CompleteMission(int score)
    {
        missionFinished = true;
        endResultTitle.text = "Mission Completed";

        if (!missionData.isCompleted)
        {
            playerStats.progressStep = missionData.advenceToStep;
            missionData.score = score;
        }

        if (missionData.score < score)
        {
            missionData.score = score;
        }

        StartCoroutine(AnimateScoreImages(score));
        ShowMissionResult();

        missionData.isCompleted = true;
    }

    private IEnumerator AnimateScoreImages(int score)
    {
        int startIndex = score > 5 ? 5 : 0;

        for (int i = startIndex; i <= Mathf.Clamp(score - 1, 0, scoreStarSprites.Length - 1); i++)
        {
            scoreImage.sprite = scoreStarSprites[i];
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void FailMission()
    {
        missionFinished = true;
        endResultTitle.text = "Mission Failed";
        scoreImage.gameObject.SetActive(false);

        if (!missionData.isCompleted)
        {
            if (!missionData.requireCompletionToAdvence)
            {
                playerStats.progressStep = missionData.advenceToStep;
            }

            if (missionData.gameOverOnFailure)
            {
                ShowGameOverScreen();
            }
        }

        ShowMissionResult();
    }

    private void ShowMissionResult()
    {
        endResultView.SetActive(true);

        float moneyResult = 0f;

        if (missionData.isCompleted)
        {
            paymentText.text = missionData.replayReward + "$";
            moneyResult += missionData.replayReward;
        }
        else
        {
            paymentText.text = missionData.reward + "$";
            moneyResult += missionData.reward;
        }

        bonusText.text = GetBonusesText();
        moneyResult += GetBonuses();

        repairText.text = "-" + Mathf.Round(repairCost).ToString() + "$";
        moneyResult -= Mathf.Round(repairCost);

        ammoCostText.text = "-" + Mathf.Round(ammoCost).ToString() + "$";
        moneyResult -= Mathf.Round(ammoCost);

        PenaltyText.text = GetPenaltiesText();
        moneyResult -= GetPenalties();

        playerStats.money += Mathf.RoundToInt(moneyResult);
    }

    private void ShowGameOverScreen()
    {
        // TODO: show game over screen, explain to reload previous save
        GameManager.Instance.LoadSceneByName("Main Menu");
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

    public void AddBonus(int amount, string description)
    {
        if (bonuses.ContainsKey(description))
        {
            bonuses[description] += amount;
        }
        else
        {
            bonuses[description] = amount;
        }
    }

    public float GetBonuses()
    {
        float total = 0;

        foreach (var bonus in bonuses)
        {
            total += bonus.Value;
        }

        return total;
    }

    public string GetBonusesText()
    {
        string bonusSummary = "";

        foreach (var bonus in bonuses)
        {
            bonusSummary += bonus.Value + "$ (" + bonus.Key + ")\n";
        }

        return bonusSummary;
    }


    public void AddPenalty(int amount, string description)
    {
        if (penalties.ContainsKey(description))
        {
            penalties[description] += amount;
        }
        else
        {
            penalties[description] = amount;
        }
    }

    public float GetPenalties()
    {
        float total = 0;

        foreach (var penalty in penalties)
        {
            total += penalty.Value;
        }

        return total;
    }

    public string GetPenaltiesText()
    {
        string penaltySummary = "";

        foreach (var penalty in penalties)
        {
            penaltySummary += "-" + penalty.Value + "$ (" + penalty.Key + ")\n";
        }

        return penaltySummary;
    }

    public void AddAmmoCost(float cost)
    {
        ammoCost += cost;
    }

    public void AddRepairCost(float cost)
    {
        repairCost += cost;
    }
}
