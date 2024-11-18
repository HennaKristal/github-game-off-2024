using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyHealth parentEnemyHealth;
    [SerializeField] private MissionController missionController;
    [SerializeField] private float health = 50;
    [SerializeField] private float collisionDamage = 100;
    [SerializeField] private int killReward = 0;
    [SerializeField] private int killPenalty = 0;
    private Animator animator;
    private Collider2D enemyCollider;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();

        if (missionController == null)
        {
            missionController = GameObject.Find("MissionController").GetComponent<MissionController>();
        }
    }

    public void TakeDamage(float damageAmount, bool redirected = false)
    {
        if (isDead)
        {
            return;
        }

        health -= damageAmount;

        if (parentEnemyHealth != null && !redirected)
        {
            parentEnemyHealth.TakeDamage(damageAmount, true);
        }

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;

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

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    // Called from animator
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public float GetCollisionDamage()
    {
        return collisionDamage;
    }
}
