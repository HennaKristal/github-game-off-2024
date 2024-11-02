using UnityEngine;


[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    public float fireRate = 0.5f;
    public float projectileSpeed = 10f;
    public int minDamage = 10;
    public int maxDamage = 15;
    public int criticalChance = 15;
    public int criticalDamageMultiplier = 2;
    public GameObject projectilePrefab;


    /* other stats
    reloadSpeed
    magazineSize
    */

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // if (moveSpeed < 0) Debug.LogWarning("Movement speed is set to 0, the player can not move", this);
    }
    #endif
}
