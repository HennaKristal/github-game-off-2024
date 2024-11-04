using UnityEngine;

[CreateAssetMenu(menuName = "Create New Plane Part")]
public class PlaneStats : ScriptableObject
{
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public int purchasePrice = 25000;
    public int sellPrice = 10000;

    [Header("Health")]
    public float maxHealth = 1200f;
    public float physicalDefence = 500f;
    public float energyDefence = 350f;

    [Header("Heating")]
    public float maxHeatTolerance = 100f;
    public float defaultHeat = 25f;

    [Header("Weight")]
    public float maxCarryWeight = 10000f;
    public float weight = 5000f;

    [Header("Energy")]
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float repairCost = 10000f;
}
