using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public float maxHealth = 1200f;
    public float physicalDefence = 500f;
    public float energyDefence = 350f;

    [Header("Speed")]
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;

    [Header("Heating")]
    public float maxHeatTolerance = 100f;
    public float defaultHeat = 25f;
    public float coolingEfficiency = 2f;
    public float overHeatcoolingEfficiency = 3f;

    [Header("Weight")]
    public float maxWeight = 10000f;

    [Header("Energy")]
    public float energyOutput = 1000;
    public float maxEnergy = 100f;
    public float energyRecharge = 2f;

    [Header("Cost")]
    public float repairCost = 10000f;
}
