using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;

    private Slider healthSlider;
    private TextMeshProUGUI healthText;
    private Slider energySlider;
    private TextMeshProUGUI energyText;
    private TextMeshProUGUI heatText;
    private float currentHealth;
    private float currentEnergy;
    private float currentHeat;
    private bool isOverheated = false;

    private void Start()
    {
        healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        healthText = GameObject.Find("Health Bar Text").GetComponent<TextMeshProUGUI>();
        energySlider = GameObject.Find("Energy Bar").GetComponent<Slider>();
        energyText = GameObject.Find("Energy Bar Text").GetComponent<TextMeshProUGUI>();
        heatText = GameObject.Find("Heat Display").GetComponent<TextMeshProUGUI>();

        currentHealth = playerStats.maxHealth;
        currentEnergy = playerStats.maxEnergy;
        currentHeat = playerStats.idleHeat;

        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = currentHealth;

        energySlider.maxValue = playerStats.maxEnergy;
        energySlider.value = currentEnergy;

        UpdateHealthUI();
        UpdateEnergyUI();
    }

    private void Update()
    {
        RechargeEnergy(Time.deltaTime);
        CoolDownHeat(Time.deltaTime);
        UpdateHealthUI();
        UpdateEnergyUI();

        if (isOverheated)
        {
            ApplyOverheatDamage(Time.deltaTime);
        }
    }

    private void RechargeEnergy(float deltaTime)
    {
        if (currentEnergy < playerStats.maxEnergy)
        {
            currentEnergy += playerStats.energyRecharge * deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, playerStats.maxEnergy);
        }
    }

    public void IncreaseHeat(float heatAmount)
    {
        currentHeat += heatAmount;
        UpdateHeatUI();

        if (currentHeat >= playerStats.maxHeatTolerance)
        {
            isOverheated = true;
        }
    }

    private void CoolDownHeat(float deltaTime)
    {
        float coolingRate = isOverheated ? playerStats.overHeatcoolingEfficiency : playerStats.coolingEfficiency;

        if (currentHeat > playerStats.idleHeat)
        {
            currentHeat -= coolingRate * deltaTime;
            currentHeat = Mathf.Max(currentHeat, playerStats.idleHeat);

            if (currentHeat < playerStats.maxHeatTolerance)
            {
                isOverheated = false;
            }
        }

        UpdateHeatUI();
    }

    private void ApplyOverheatDamage(float deltaTime)
    {
        currentHealth -= 1f * deltaTime;
        currentHealth = Mathf.Max(currentHealth, 0);
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
        healthText.text = Mathf.Ceil(currentHealth).ToString();
    }

    private void UpdateEnergyUI()
    {
        energySlider.value = currentEnergy;
        energyText.text = Mathf.Floor(currentEnergy).ToString();
    }

    private void UpdateHeatUI()
    {
        if (currentHeat <= playerStats.idleHeat)
        {
            heatText.color = new Color(1f, 1f, 0f);
            heatText.text = Mathf.Floor(currentHeat).ToString() + "ºC";
        }
        else if (currentHeat >= playerStats.maxHeatTolerance)
        {
            heatText.color = new Color(1f, 0f, 0f);
            heatText.text = "! " + Mathf.Floor(currentHeat).ToString() + "ºC";
        }
        else
        {
            float t = (currentHeat - playerStats.idleHeat) / (playerStats.maxHeatTolerance - playerStats.idleHeat);
            heatText.color = Color.Lerp(new Color(1f, 1f, 0f), new Color(1f, 0f, 0f), t);
            heatText.text = Mathf.Floor(currentHeat).ToString() + "ºC";
        }
    }
}
