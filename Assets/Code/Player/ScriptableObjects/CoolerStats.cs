using UnityEngine;

[CreateAssetMenu]
public class CoolerStats : ScriptableObject
{
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public int purchasePrice = 25000;
    public int sellPrice = 10000;

    [Header("Heating")]
    public float coolingEfficiency = 2f;
    public float overHeatcoolingEfficiency = 3f;

    [Header("Weight")]
    public float weight = 1000f;

    [Header("Energy")]
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float repairCost = 10000f;
}