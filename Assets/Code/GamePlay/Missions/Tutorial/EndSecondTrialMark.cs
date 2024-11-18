using UnityEngine;

public class EndSecondTrialMark : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialController.EndSecondTrial();
            this.enabled = false;
        }
    }
}
