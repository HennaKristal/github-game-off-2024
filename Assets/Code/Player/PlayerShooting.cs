using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public enum WeaponType
{
    MainWeapon,
    LeftInner,
    LeftOuter,
    RightInner,
    RightOuter
}

public class PlayerShooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponType weaponType;
    private MissionController missionController;
    private InputController inputController;
    private WeaponStats weaponStats;

    [Header("UI")]
    private TextMeshProUGUI weaponNameText;
    private TextMeshProUGUI magazineText;
    private TextMeshProUGUI totalRoundsText;
    private TextMeshProUGUI separator;
    private Slider reloadBar;

    private float nextFireTime = 0f;
    private int currentMagazine;
    private int remainingRounds;
    private bool isReloading = false;

    private void Start()
    {
        string parentName = weaponType switch
        {
            WeaponType.MainWeapon => "Main Gun Ammo",
            WeaponType.LeftInner => "Left Inner Weapon Ammo",
            WeaponType.LeftOuter => "Left Outer Weapon Ammo",
            WeaponType.RightInner => "Right Inner Weapon Ammo",
            WeaponType.RightOuter => "Right Outer Weapon Ammo",
            _ => null

        };

        missionController = GameObject.Find("MissionController").GetComponent<MissionController>();

        weaponNameText = GameObject.Find(parentName).transform.Find("Name").GetComponent<TextMeshProUGUI>();
        magazineText = GameObject.Find(parentName).transform.Find("Magazine").GetComponent<TextMeshProUGUI>();
        totalRoundsText = GameObject.Find(parentName).transform.Find("Total Rounds").GetComponent<TextMeshProUGUI>();
        separator = GameObject.Find(parentName).transform.Find("Separator").GetComponent<TextMeshProUGUI>();
        reloadBar = GameObject.Find(parentName).transform.Find("Reload Bar").GetComponent<Slider>();

        weaponStats = GetWeaponStats();

        if (weaponStats == null)
        {
            this.enabled = false;
        }

        inputController = GameManager.Instance.GetComponent<InputController>();

        weaponNameText.text = weaponStats.partName;

        currentMagazine = weaponStats.magazineSize;
        magazineText.text = currentMagazine.ToString();

        remainingRounds = weaponStats.totalRounds;
        totalRoundsText.text = remainingRounds.ToString();
    }

    private WeaponStats GetWeaponStats()
    {
        switch (weaponType)
        {
            case WeaponType.MainWeapon: return GameManager.Instance.allMainWeaponsParts.FirstOrDefault(w => w.isEquipped);
            case WeaponType.LeftInner: return GameManager.Instance.allLeftInnerWeaponParts.FirstOrDefault(w => w.isEquipped);
            case WeaponType.LeftOuter: return GameManager.Instance.allLeftOuterWeaponParts.FirstOrDefault(w => w.isEquipped);
            case WeaponType.RightInner: return GameManager.Instance.allRightInnerWeaponParts.FirstOrDefault(w => w.isEquipped);
            case WeaponType.RightOuter: return GameManager.Instance.allRightOuterWeaponParts.FirstOrDefault(w => w.isEquipped);
            default: return null;
        }
    }

    private void Update()
    {
        if (isReloading) return;

        if (IsFireButtonPressed())
        {
            if (inputController.reloadHeld && remainingRounds > 0)
            {
                StartCoroutine(Reload());
            }
            else if (Time.time >= nextFireTime && currentMagazine > 0)
            {
                FireWeapon();
                nextFireTime = Time.time + (1f / weaponStats.fireRate);
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
                    weaponNameText.color = Color.red;
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
                WeaponType.MainWeapon => inputController.MainWeaponHeld,
                WeaponType.LeftInner => inputController.LeftInnerWeaponHeld,
                WeaponType.RightInner => inputController.RightInnerWeaponHeld,
                WeaponType.LeftOuter => inputController.LeftOuterWeaponHeld,
                WeaponType.RightOuter => inputController.RightOuterWeaponHeld,
                _ => false,
            };
        }
        else
        {
            return weaponType switch
            {
                WeaponType.MainWeapon => inputController.MainWeaponHeld,
                WeaponType.LeftInner => inputController.LeftInnerWeaponHeld,
                WeaponType.RightInner => inputController.RightInnerWeaponHeld,
                WeaponType.LeftOuter => inputController.LeftOuterWeaponHeld,
                WeaponType.RightOuter => inputController.RightOuterWeaponHeld,
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

            missionController.AddAmmoCost(weaponStats.ammunitionCost);
            playerController.IncreaseHeat(weaponStats.heating);
        }
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

        weaponNameText.text = weaponStats.partName;
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
