using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject missionHolder;
    [SerializeField] private Sprite[] missionScores;
    [SerializeField] private Sprite[] missionDifficulties;
    private InputController inputController;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionEmployer;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private Image missionDifficultyImage;
    [SerializeField] private Image missionScoreImage;
    [SerializeField] private TextMeshProUGUI missionRewardText;
    [SerializeField] private TextMeshProUGUI missionAdditionalInfoText;
    [SerializeField] private GameObject replayMissionsButton;
    [SerializeField] private GameObject returnToGarageButton;
    [SerializeField] private GameObject launchMissionButton;
    [SerializeField] private GameObject rejectMissionButton;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip pressedSound;

    private List<MissionStats> activeMissions = new List<MissionStats>();
    private List<Image> missionTargetImages = new List<Image>();
    private List<Transform> missionTargetTransforms = new List<Transform>();
    private bool isDisplayingReplayMission = false;
    private int currentIndex = 0;
    private int currentCategory = 1;
    private int missionActionSelectIndex = 0;
    private float inputCooldown = 0.25f;
    private float nextInputTime = 0f;
    private float movementDeadZone = 0.4f;

    private void Start()
    {
        inputController = GameManager.Instance.GetInputController();

        ShowStoryMissions();
        UpdateSelectedElement();
    }

    private void Update()
    {
        HandleNavigation();

        if (inputController.dodgePressed)
        {
            if (currentCategory == 1)
            {
                Invoke(nameof(SelectMission), 0f);
            }
            else if (currentCategory == 2)
            {
                if (isDisplayingReplayMission)
                {
                    replayMissionsButton.GetComponent<TextMeshProUGUI>().text = "Replay Missions";
                    Invoke(nameof(ShowStoryMissions), 0f);
                }
                else
                {
                    replayMissionsButton.GetComponent<TextMeshProUGUI>().text = "Continue Story";
                    Invoke(nameof(ShowReplayMissions), 0f);
                }
            }
            else if (currentCategory == 3)
            {
                ReturnToGarage();
            }
            else if (currentCategory == 4)
            {
                if (missionActionSelectIndex == 1)
                {
                    LaunchMission();
                }
                else
                {
                    RejectMission();
                }
            }

            PlayPressedSound();
        }

        if (inputController.healPressed && currentCategory == 4)
        {
            RejectMission();
            PlayPressedSound();
        }
    }

    private void HandleNavigation()
    {
        // Reset delay if input is released
        if (Mathf.Abs(inputController.Move.x) < movementDeadZone && Mathf.Abs(inputController.Move.y) < movementDeadZone)
        {
            nextInputTime = Time.time;
        }

        // Delay for inputs
        if (Time.time < nextInputTime)
        {
            return;
        }

        int previousIndex = currentIndex;
        int previousCategory = currentCategory;
        int previousmissionActionSelectIndex = missionActionSelectIndex;

        // Horizontal movement (right)
        if (inputController.Move.x > movementDeadZone)
        {
            if (currentCategory == 1 && currentIndex < activeMissions.Count - 1)
            {
                currentIndex++;
            }
            else if (currentCategory == 4)
            {
                missionActionSelectIndex = 1;
            }
        }
        // Horizontal movement (left)
        else if (inputController.Move.x < -movementDeadZone)
        {
            if (currentCategory == 1 && currentIndex > 0)
            {
                currentIndex--;
            }
            else if (currentCategory == 4)
            {
                missionActionSelectIndex = 0;
            }
        }
        // Vertical movement (up)
        else if (inputController.Move.y > movementDeadZone)
        {
            if (currentCategory == 3)
            {
                currentCategory = 2;
            }
            else if (currentCategory == 2)
            {
                currentCategory = 1;
            }
        }
        // Vertical movement (down)
        else if (inputController.Move.y < -movementDeadZone)
        {
            if (currentCategory == 1)
            {
                currentCategory = 2;
            }
            else if (currentCategory == 2)
            {
                currentCategory = 3;
            }
        }

        // Update slots if something changed
        if (currentIndex != previousIndex || currentCategory != previousCategory || missionActionSelectIndex != previousmissionActionSelectIndex)
        {
            nextInputTime = Time.time + inputCooldown;
            PlaySelectionSound();
            UpdateSelectedElement();
        }
    }

    private void UpdateSelectedElement()
    {
        // Reset Buttons
        replayMissionsButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
        returnToGarageButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

        foreach (Image missionTargetImage in missionTargetImages)
        {
            missionTargetImage.color = new Color(1f, 1f, 1f);
        }

        foreach (Transform missionTargetTransform in missionTargetTransforms)
        {
            missionTargetTransform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Update current element
        if (currentCategory == 1)
        {
            missionTargetImages[currentIndex].color = new Color(1f, 0.66f, 0f);
            missionTargetTransforms[currentIndex].localScale = new Vector3(1.25f, 1.25f, 1.25f);

            missionNameText.text = activeMissions[currentIndex].missionName;
            missionEmployer.text = activeMissions[currentIndex].employer;
            missionDescriptionText.text = activeMissions[currentIndex].description;
            missionAdditionalInfoText.text = activeMissions[currentIndex].additionalInformation;
            missionDifficultyImage.sprite = missionDifficulties[activeMissions[currentIndex].difficulty - 1];
       

            if (activeMissions[currentIndex].isCompleted)
            {
                missionScoreImage.gameObject.SetActive(true);
                missionScoreImage.sprite = missionScores[activeMissions[currentIndex].score];
                missionRewardText.text = "Reward: " + activeMissions[currentIndex].replayReward + "$";
            }
            else
            {
                missionScoreImage.gameObject.SetActive(false);
                missionRewardText.text = "Reward: " + activeMissions[currentIndex].reward + "$";
            }
        }
        else if (currentCategory == 2)
        {
            replayMissionsButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
        else if (currentCategory == 3)
        {
            returnToGarageButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
        }
        else if (currentCategory == 4)
        {
            if (missionActionSelectIndex == 0)
            {
                launchMissionButton.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);
                rejectMissionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 0f, 0f);
            }
            else
            {
                launchMissionButton.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0f);
                rejectMissionButton.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void ClearMissions()
    {
        currentIndex = 0;

        activeMissions.Clear();
        missionTargetImages.Clear();
        missionTargetTransforms.Clear();

        foreach (Transform child in missionHolder.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void ShowStoryMissions()
    {
        ShowMissions(false);
    }

    private void ShowReplayMissions()
    {
        ShowMissions(true);
    }

    private void ShowMissions(bool showCompleted)
    {
        ClearMissions();

        isDisplayingReplayMission = showCompleted;

        foreach (MissionStats mission in GameManager.Instance.missions)
        {
            if ((showCompleted && mission.isCompleted) || (!showCompleted && playerStats.progressStep == mission.step && !mission.isCompleted))
            {
                Transform missionObject = missionHolder.transform.Find(mission.missionName);

                if (missionObject == null)
                {
                    Debug.LogWarning($"Mission with name {mission.missionName} not found in missionHolder!");
                    continue;
                }

                missionObject.gameObject.SetActive(true);
                activeMissions.Add(mission);
                missionTargetImages.Add(missionObject.Find("Mission Target Icon").GetComponent<Image>());
                missionTargetTransforms.Add(missionObject.Find("Mission Target Icon"));
            }
        }
    }

    private void SelectMission()
    {
        currentCategory = 4;
        launchMissionButton.SetActive(true);
        rejectMissionButton.SetActive(true);

        missionActionSelectIndex = 1;
        UpdateSelectedElement();
    }

    private void LaunchMission()
    {
        playerStats.playingNow = activeMissions[currentIndex].sceneName;
        GameManager.Instance.LoadSceneByName(activeMissions[currentIndex].sceneName);
        this.enabled = false;
    }

    private void RejectMission()
    {
        currentCategory = 1;
        launchMissionButton.SetActive(false);
        rejectMissionButton.SetActive(false);
    }

    private void ReturnToGarage()
    {
        GameManager.Instance.LoadSceneByName("Garage");
        this.enabled = false;
    }

    private void PlaySelectionSound()
    {
        audioSource.PlayOneShot(selectionSound);
    }

    private void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound);
    }
}
