using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [Header("General")]
    public int money = 100000;
    public GameObject playerPrefab;

    [Header("Health")]
    public float maxHealth = 1200f;
    public float physicalDefence = 500f;
    public float energyDefence = 350f;

    [Header("Speed")]
    public float horizontalSpeed = 5f;
    public float horizontalIdleSpeed = 1f;
    public float horizontalReverseSpeed = 2.5f;
    public float verticalSpeed = 5f;

    [Header("Heating")]
    public float maxHeatTolerance = 100f;
    public float idleHeat = 25f;
    public float coolingEfficiency = 2f;
    public float overHeatcoolingEfficiency = 3f;

    [Header("Weight")]
    public float currentCarryWeight = 5000f;
    public float maxCarryWeight = 10000f;
    public float currentLiftWeight = 5000f;
    public float maxLiftWeight = 15000f;

    [Header("Energy")]
    public float energyOutput = 1000;
    public float maxEnergy = 100f;
    public float energyRecharge = 2f;
    public float energyConsumption = 50f;

    [Header("Cost")]
    public float repairCost = 10000f;
}
