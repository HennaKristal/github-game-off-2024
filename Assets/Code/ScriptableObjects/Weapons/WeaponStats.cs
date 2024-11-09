using UnityEngine;

[CreateAssetMenu(menuName = "Create New weapon")]
public class WeaponStats : ScriptableObject
{
    public string partName = "";
    [TextArea(3, 50)] public string description = "";
    public string manufacturer = "";
    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public int purchasePrice = 25000;
    public int sellPrice = 10000;

    [Header("Projectile")]
    public GameObject projectilePrefab;

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
    public float enegryCost = 0f;

    [Header("Heating")]
    public float heating = 0.01f;

    [Header("Weight and Energy")]
    public float weight = 500f;
    public float energyConsumption = 33f;

    [Header("Cost")]
    public float ammunitionCost = 1f;
}
