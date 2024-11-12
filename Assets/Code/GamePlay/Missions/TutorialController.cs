using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private MissionController missionController;
    [SerializeField] private BadgeStats badgeReward;
    public float timeToComplete = 10f;

    private void Start()
    {
        Invoke("FinishMission", timeToComplete);
    }

    private void FinishMission()
    {
        GameManager.Instance.AddNewNotification("Welcome in!", "Secret Sanctum", "TODO: Great to have you with us! Now that you have proven yourself let me officially invote you to our ransk. We have granted you your furst badge of honor, please check your badges and wear it proudly. Once you are ready, meet me at the command sector and we will assign your next task for you.");
        GameManager.Instance.AddNewNotification("Instroduction to garage", "System", "TODO: This is your garage. Here you can manage your plane by editing the parts it is build of.");

        if (badgeReward != null)
        {
            badgeReward.isOwned = true;
        }

        missionController.CompleteMission(6);
    }
}
