using UnityEngine;

[CreateAssetMenu(menuName = "Create New Engine Part")]
public class EngineStats : ScriptableObject
{
    public string partName = "";
    public Sprite image;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public int purchasePrice = 25000;
    public int sellPrice = 10000;

    [Header("Speed")]
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;

    [Header("Weight")]
    public float maxLiftWeight = 15000f;
    public float weight = 1000f;

    [Header("Energy")]
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float repairCost = 10000f;
}
