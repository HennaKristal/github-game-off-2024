using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private MissionController missionController;

    private void Start()
    {
        Invoke("FinishMission", 10f);
    }

    private void FinishMission()
    {
        missionController.CompleteMission(1);
    }
}
