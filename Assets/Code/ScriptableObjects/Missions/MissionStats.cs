using UnityEngine;

[CreateAssetMenu(menuName = "Create New Mission")]
public class MissionStats : ScriptableObject
{
    public string missionName = "";
    [TextArea(3, 50)] public string description = "";

    public bool isCompleted = false;

    public int step = 0;
    public int advenceToStep = 0;

    public int reward = 15000;
    public int replayReward = 10000;
    [TextArea(3, 50)] public string additionalInformation = "";


    public string sceneName = "";
}
