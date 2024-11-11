using UnityEngine;

[CreateAssetMenu(menuName = "Create New Mission")]
public class MissionStats : ScriptableObject
{
    public string missionName = "";
    public string sceneName = "";
    public string employer = "";
    [TextArea(3, 50)] public string description = "";
    [TextArea(3, 50)] public string additionalInformation = "";

    public bool isCompleted = false;

    [Range(1, 10)] public int difficulty = 1;
    [Range(0, 10)] public int score = 0;

    public int step = 0;
    public int advenceToStep = 0;
    public bool requireCompletionToAdvence = true;
    public bool gameOverOnFailure = false;

    public int reward = 15000;
    public int replayReward = 10000;
}
