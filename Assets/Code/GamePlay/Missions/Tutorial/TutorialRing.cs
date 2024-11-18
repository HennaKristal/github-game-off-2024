using UnityEngine;

public class TutorialRing : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;
    [SerializeField] private SpriteRenderer ringPart1;
    [SerializeField] private SpriteRenderer ringPart2;
    [SerializeField] private EnemyHealth enemyHealth;
    private bool disabled = false;
    private bool pointGiven = false;
    private bool isFailed = false;

    public void Update()
    {
        if (!isFailed && enemyHealth.IsDead())
        {
            disabled = true;
            isFailed = true;
            ringPart1.color = Color.red;
            ringPart2.color = Color.red;

            if (pointGiven)
            {
                tutorialController.loopsPassed--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disabled && collision.CompareTag("Player"))
        {
            if (!enemyHealth.IsDead())
            {
                disabled = true;
                ringPart1.color = Color.green;
                ringPart2.color = Color.green;
                tutorialController.loopsPassed++;
                pointGiven = true;
            }

        }
    }
}
