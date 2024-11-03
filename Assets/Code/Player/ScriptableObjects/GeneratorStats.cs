using UnityEngine;

[CreateAssetMenu]
public class GeneratorStats : ScriptableObject
{
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public int purchasePrice = 25000;
    public int sellPrice = 10000;

    [Header("Weight")]
    public float weight = 1000f;

    [Header("Energy")]
    public float energyOutput = 1000;
    public float maxEnergy = 100f;
    public float energyRecharge = 2f;
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float repairCost = 10000f;
}
