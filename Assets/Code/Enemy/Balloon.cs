using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float activationRange = 20f;

    private EnemyHealth enemyHealth;
    private Transform player;
    private Vector3 startPosition;

    private bool shouldFloat = false;
    private bool isPopped = false;

    private void Start()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        startPosition = transform.position;
    }

    private void Update()
    {
        if (shouldFloat)
        {
            transform.Translate(Vector2.up * floatSpeed * Time.deltaTime);
        }
        else if (startPosition.x - player.position.x < activationRange)
        {
            shouldFloat = true;
        }

        if (!isPopped && enemyHealth.IsDead())
        {
            isPopped = true;
            tutorialController.balloonsPopped++;
        }
    }
}

