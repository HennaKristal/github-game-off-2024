using UnityEngine;

public class EndThirdTutorialMark : MonoBehaviour
{
    [SerializeField] private TutorialController tutorialController;
    [SerializeField] private BossBalloon bossBalloon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossBalloon.FlyAway();
            tutorialController.EndThirdTrial(false);
            this.enabled = false;
        }
    }
}
