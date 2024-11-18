using UnityEngine;

public class EndFirstTrialMark : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialController.EndFirstTrial();
            this.enabled = false;
        }
    }
}
