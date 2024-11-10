using UnityEngine;

[CreateAssetMenu(menuName = "Create New Mission")]
public class MissionStats : ScriptableObject
{
    public string missionName = "";
    [TextArea(3, 50)] public string description = "";
    public string employer = "";

    public bool isCompleted = false;

    [Range(1, 10)] public int difficulty = 1;
    [Range(0, 10)] public int score = 0;
    public int step = 0;
    public int advenceToStep = 0;
    public bool requireCompletionToAdvence = true;
    public bool gameOverOnFailure = false;


    public int reward = 15000;
    public int replayReward = 10000;
    [TextArea(3, 50)] public string additionalInformation = "";


    public string sceneName = "";
}
