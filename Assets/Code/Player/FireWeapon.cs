using UnityEngine;


public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private WeaponStats weaponStats;
    [SerializeField] private string fireInputKey;
    private float nextFireTime;


    private void Start()
    {
        nextFireTime = 0f;
    }


    private void Update()
    {
        if (IsFireButtonPressed() && Time.time >= nextFireTime)
        {
            FireWeapon();
            nextFireTime = Time.time + weaponStats.fireRate;
        }
    }


    private bool IsFireButtonPressed()
    {
        return fireInputKey switch
        {
            "fire1Hold" => inputController.fire1Hold,
            "fire2Hold" => inputController.fire2Hold,
            "fire3Hold" => inputController.fire3Hold,
            "fire4Hold" => inputController.fire4Hold,
            "fire5Hold" => inputController.fire5Hold,
            _ => false,
        };
    }


    private void FireWeapon()
    {
        GameObject projectile = Instantiate(weaponStats.projectilePrefab, transform.position, transform.rotation);

        Bullet bullet = projectile.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Initialize(weaponStats.projectileSpeed, weaponStats.minDamage, weaponStats.maxDamage, weaponStats.criticalChance, weaponStats.criticalDamageMultiplier);
        }
    }
}
