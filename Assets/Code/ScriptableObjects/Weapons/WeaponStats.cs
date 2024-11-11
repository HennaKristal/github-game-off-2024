using UnityEngine;

[CreateAssetMenu(menuName = "Create New weapon")]
public class WeaponStats : ScriptableObject
{
    public string partName = "";
    public string saveName = "";
    public string manufacturer = "";
    [TextArea(3, 50)] public string description = "";

    public Sprite icon;

    [Header("Ownership")]
    public bool isOwned = false;
    public bool isEquipped = false;
    public bool isPurchasable = false;
    public bool isOwnedByDefault = false;
    public bool isEquippedByDefault = false;
    public bool isPurchasableByDefault = false;
    public int purchasePrice = 0;
    public int sellPrice = 0;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Damage")]
    public int minDamage = 0;
    public int maxDamage = 0;
    public float criticalChance = 0;
    public float criticalDamageMultiplier = 1;
    public float fireRate = 1;
    public bool isAuto = true;
    public float projectileSpeed = 0f;

    [Header("Ammo")]
    public int totalRounds = 0;
    public int magazineSize = 0;
    public float reloadSpeed = 0f;
    public float enegryCost = 0f;

    [Header("Heating")]
    public float heating = 0f;

    [Header("Weight and Energy")]
    public float weight = 0f;
    public float energyConsumption = 0f;

    [Header("Cost")]
    public float ammunitionCost = 0f;
}
