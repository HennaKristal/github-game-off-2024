using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private MissionController missionController;
    [SerializeField] private int health = 50;
    [SerializeField] private int killReward = 0;
    [SerializeField] private int killPenalty = 0;
    private bool isDead = false;

    private void Start()
    {
        if (missionController == null)
        {
            missionController = GameObject.Find("MissionController").GetComponent<MissionController>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (missionController != null)
        {
            if (killReward > 0)
            {
                missionController.AddBonus(killReward, "Enemy Target Destoryed");
            }

            if (killPenalty > 0)
            {
                missionController.AddPenalty(killPenalty, "Ally Killed");
            }
        }

        Destroy(gameObject);
    }
}
