using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 50;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
