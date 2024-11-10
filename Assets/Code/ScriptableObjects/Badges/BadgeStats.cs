using UnityEngine;

[CreateAssetMenu(menuName = "Create New Badge")]
public class BadgeStats : ScriptableObject
{
    public string partName = "";
    public string manufacturer = "";
    [TextArea(3, 50)] public string description = "";

    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isOwnedByDefault = false;
    public bool isEquippedByDefault = false;
}
