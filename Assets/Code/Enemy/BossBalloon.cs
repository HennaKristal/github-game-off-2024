using UnityEngine;

public class BossBalloon : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private float activationRange = 20f;
    [SerializeField] private float floatAmplitude = 2f;
    [SerializeField] private float floatFrequency = 1f;

    private EnemyHealth enemyHealth;
    private Transform player;
    private Vector3 startPosition;
    private float speed;

    private bool shouldFloat = false;
    private bool isPopped = false;
    private bool flewAway = false;

    private void Start()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        startPosition = transform.position;
        speed = cameraMovement.speed;
    }

    public void FlyAway()
    {
        speed += 1f;
        flewAway = true;
    }

    private void Update()
    {
        if (shouldFloat)
        {
            // Horizontal movement with camera speed
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (startPosition.x - player.position.x < activationRange)
        {
            shouldFloat = true;
        }

        // Add vertical oscillation (up and down)
        float oscillation = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + oscillation, transform.position.z);

        // Handle popping logic
        if (!flewAway && !isPopped && enemyHealth.IsDead())
        {
            isPopped = true;
            tutorialController.EndThirdTrial(true);
        }
    }
}
