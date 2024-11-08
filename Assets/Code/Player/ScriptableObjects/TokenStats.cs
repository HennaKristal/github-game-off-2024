using UnityEngine;

[CreateAssetMenu(menuName = "Create New Token")]
public class TokenStats : ScriptableObject
{
    public string partName = "";
    [TextArea(3, 50)] public string description = "";
    public string manufacturer = "";
    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;    public bool isEquipped = false;
}
