using UnityEngine;

[CreateAssetMenu(menuName = "Create New Badge")]
public class BadgeStats : ScriptableObject
{
    public string partName = "";
    [TextArea(3, 50)] public string description = "";
    public string manufacturer = "";
    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
}
