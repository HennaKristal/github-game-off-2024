using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public enum WeaponType
{
    MainWeapon,
    SideWeapon1,
    SideWeapon2,
    SideWeapon3,
    SideWeapon4
}

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InputController inputController;
    [SerializeField] private WeaponStats weaponStats;
    [SerializeField] private WeaponType weaponType;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI magazineText;
    [SerializeField] private TextMeshProUGUI totalRoundsText;
    [SerializeField] private TextMeshProUGUI separator;
    [SerializeField] private Slider reloadBar; // Reload bar slider

    private float nextFireTime = 0f;
    private int currentMagazine;
    private int remainingRounds;
    private bool isReloading = false;

    private void Start()
    {
        weaponNameText.text = weaponStats.weaponName;

        currentMagazine = weaponStats.magazineSize;
        magazineText.text = currentMagazine.ToString();

        remainingRounds = weaponStats.totalRounds;
        totalRoundsText.text = remainingRounds.ToString();
    }

    private void Update()
    {
        if (isReloading) return;

        if (IsFireButtonPressed())
        {
            if (inputController.reloadHold && remainingRounds > 0)
            {
                StartCoroutine(Reload());
            }
            else if (Time.time >= nextFireTime && currentMagazine > 0)
            {
                FireWeapon();
                nextFireTime = Time.time + weaponStats.fireRate;
                currentMagazine--;
                magazineText.text = currentMagazine.ToString();
            }
            else if (currentMagazine <= 0)
            {
                magazineText.color = Color.red;

                if (remainingRounds > 0)
                {
                    magazineText.text = "-";
                    StartCoroutine(Reload());
                }
                else
                {
                    magazineText.text = "0";
                }
            }
        }
    }

    private bool IsFireButtonPressed()
    {
        if (weaponStats.isAuto)
        {
            return weaponType switch
            {
                WeaponType.MainWeapon => inputController.fire1Hold,
                WeaponType.SideWeapon1 => inputController.fire2Hold,
                WeaponType.SideWeapon2 => inputController.fire3Hold,
                WeaponType.SideWeapon3 => inputController.fire4Hold,
                WeaponType.SideWeapon4 => inputController.fire5Hold,
                _ => false,
            };
        }
        else
        {
            return weaponType switch
            {
                WeaponType.MainWeapon => inputController.fire1Pressed,
                WeaponType.SideWeapon1 => inputController.fire2Pressed,
                WeaponType.SideWeapon2 => inputController.fire3Pressed,
                WeaponType.SideWeapon3 => inputController.fire4Pressed,
                WeaponType.SideWeapon4 => inputController.fire5Pressed,
                _ => false,
            };
        }
    }

    private void FireWeapon()
    {
        GameObject projectile = Instantiate(weaponStats.projectilePrefab, transform.position, transform.rotation);
        Bullet bullet = projectile.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Initialize(
                weaponStats.projectileSpeed,
                weaponStats.minDamage,
                weaponStats.maxDamage,
                weaponStats.criticalChance,
                weaponStats.criticalDamageMultiplier
            );
        }

        playerController.IncreaseHeat(weaponStats.heating);
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        weaponNameText.text = "Reloading";
        weaponNameText.color = Color.red;
        reloadBar.gameObject.SetActive(true);
        reloadBar.maxValue = weaponStats.reloadSpeed;
        reloadBar.value = 0;

        float reloadTimeElapsed = 0f;

        // Reload
        while (reloadTimeElapsed < weaponStats.reloadSpeed)
        {
            reloadTimeElapsed += Time.deltaTime;
            reloadBar.value = reloadTimeElapsed;
            yield return null;
        }

        int roundsToLoad = Mathf.Min(weaponStats.magazineSize - currentMagazine, remainingRounds);
        currentMagazine += roundsToLoad;
        remainingRounds -= roundsToLoad;

        magazineText.color = Color.white;
        magazineText.text = currentMagazine.ToString();
        totalRoundsText.text = remainingRounds.ToString();

        weaponNameText.text = weaponStats.weaponName;
        weaponNameText.color = new Color(0.78f, 0.78f, 0.78f);
        reloadBar.gameObject.SetActive(false);

        if (remainingRounds == 0)
        {
            totalRoundsText.color = Color.red;
            separator.color = Color.red;
        }

        isReloading = false;
    }

    public void AddAmmo(int amount)
    {
        remainingRounds = Mathf.Min(remainingRounds + amount, weaponStats.totalRounds);
        totalRoundsText.text = remainingRounds.ToString();
        totalRoundsText.color = Color.white;
        separator.color = Color.white;
    }
}