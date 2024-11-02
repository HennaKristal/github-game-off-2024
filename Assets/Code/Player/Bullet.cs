using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject damageNumberPrefab;
    private Rigidbody2D rb;
    private Collider2D collider;
    private int damage;
    private float speed;
    private bool isCritical = false;
    private bool hasProcessedCollision = false;
    private bool hasStartedCollisionCheck = false;
    private EnemyWeakSpot weakSpotCollision;
    private EnemyHealth normalCollision;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }


    public void Initialize(float _speed, int _minDamage, int _maxDamage, int _criticalChance, float _criticalDamageMultiplier)
    {
        speed = _speed;
        damage = Random.Range(_minDamage, _maxDamage + 1);
        isCritical = (Random.Range(1, 101) <= _criticalChance);

        if (isCritical)
        {
            damage = Mathf.RoundToInt(damage * _criticalDamageMultiplier);
        }

        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasProcessedCollision) { return; }

        // Check if it collided with the weak spot
        weakSpotCollision = collision.GetComponent<EnemyWeakSpot>();
        if (weakSpotCollision != null)
        {
            if (!hasStartedCollisionCheck)
            {
                hasStartedCollisionCheck = true;
                StartCoroutine(ProcessCollisionAfterFrame());
            }
            return;
        }

        // Check if it collided with the main enemy body
        normalCollision = collision.GetComponent<EnemyHealth>();
        if (normalCollision != null)
        {
            if (!hasStartedCollisionCheck)
            {
                hasStartedCollisionCheck = true;
                StartCoroutine(ProcessCollisionAfterFrame());
            }
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            HandleCollisionImpact();
        }
    }


    private IEnumerator ProcessCollisionAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        hasProcessedCollision = true;

        if (weakSpotCollision != null)
        {
            weakSpotCollision.TakeDamage(damage);
            ShowDamageNumber(weakSpotCollision.GetWeakSpotDamage(damage), transform.position, true);
        }
        else if (normalCollision != null)
        {
            normalCollision.TakeDamage(damage);
            ShowDamageNumber(damage, transform.position, false);
        }

        HandleCollisionImpact();
    }


    private void HandleCollisionImpact()
    {
        Destroy(gameObject);
    }


    private void ShowDamageNumber(int damageAmount, Vector3 hitPosition, bool isWeakSpot)
    {
        GameObject damageNumber = Instantiate(damageNumberPrefab, hitPosition, Quaternion.identity);
        damageNumber.GetComponent<DamageNumber>().Initialize(damageAmount, isCritical, isWeakSpot);
    }
}
