using UnityEngine;

public class EnemyWeakSpot : MonoBehaviour
{
    [SerializeField] private float weakSpotDamageMultiplier = 2;
    private EnemyHealth enemyHealth;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyWeakSpot requires an EnemyHealth component on the parent GameObject.");
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(Mathf.RoundToInt(damage * weakSpotDamageMultiplier));
    }

    public float GetWeakSpotMultiplier()
    {
        return weakSpotDamageMultiplier;
    }

    public int GetWeakSpotDamage(int damage)
    {
        return Mathf.RoundToInt(damage * weakSpotDamageMultiplier);
    }
}
