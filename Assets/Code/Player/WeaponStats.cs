using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    [Header("Prefab")]
    public GameObject projectilePrefab;
    public string weaponName = "";

    [Header("Damage")]
    public int minDamage = 35;
    public int maxDamage = 50;
    public float criticalChance = 15;
    public float criticalDamageMultiplier = 1.75f;
    public float fireRate = 0.5f;
    public bool isAuto = true;
    public float projectileSpeed = 10f;

    [Header("Ammo")]
    public int totalRounds = 250;
    public int magazineSize = 30;
    public float reloadSpeed = 3f;
    public float EnegryCost = 0f;

    [Header("Heating")]
    public float heating = 0.01f;

    [Header("Weight and Energy")]
    public float weight = 500f;
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float ammunitionCost = 1f;
}
