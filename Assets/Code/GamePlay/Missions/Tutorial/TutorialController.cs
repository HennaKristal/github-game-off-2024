using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private MissionStats missionStats;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private MissionController missionController;
    [SerializeField] private BadgeStats badgeRewardBeginner;
    [SerializeField] private BadgeStats badgeRewardElite;
    public int loopsPassed = 0;
    public int balloonsPopped = 0;
    private float score = 0;

    private void Start()
    {
        dialogueManager.Add(
            "Tutor Vega",
            "Secret Sanctum",
            "Welcome, mercenary. It’s time to see if you have what it takes to join the ranks of Secret Sanctum. Complete this evaluation, and you’ll earn your place as one of us.",
            10f
        );
       
        dialogueManager.Add(
            "Tutor Vega",
            "Secret Sanctum",
            "Your first task is simple, fly through the rings ahead and prove you can handle this iron beast without turning it into a pile of scrap metal.",
            8f
        );
    }

    public void EndFirstTrial()
    {
        string message;

        if (loopsPassed == 0)
        {
            message = "Sigh- you didn’t manage to pass a single ring, but at least you didn’t crash the plane. I hope you'll do better with the next task.";
        }
        else if (loopsPassed < 3)
        {
            message = "Not bad, mercenary. You made it through a few rings. There’s still a lot to master with these iron beasts, but you might just make it.";
        }
        else if (loopsPassed < 5)
        {
            message = "Fantastic work mercenary! You flew through most of the rings with skill and precision. I am impressive, keep it up.";
        }
        else
        {
            message = "Exceptional work! You flew through those rings like they were your highway. Outstanding performance.";
        }


        dialogueManager.Add("Tutor Vega", "Secret Sanctum", message, 8f);

        dialogueManager.Add(
            "Tutor Vega",
            "Secret Sanctum",
            "Time for some action! Lets see how you handle those weapons. Your 2nd task is to shoot down the red balloons as they appear.",
            8f
        );
    }

    public void EndSecondTrial()
    {
        score = GetScore();

        if (score < 2)
        {
            dialogueManager.Add(
                   "Tutor Vega",
                   "Secret Sanctum",
                    "Thank you for participating in the evaluation, mercenary. We’ve reviewed your performance and unfortunately, you do not meet the standards required. I’m sorry, but we need to let you go.",
                    10f
               );

            Invoke(nameof(EvaluationFailed), 9f);
            return;
        }

        string message = "";

        if (balloonsPopped <= 0)
        {
            message = "You missed every single balloon, are you blind or did we hire a pacifist?";
        }
        else if (balloonsPopped < 4)
        {
            message = "At least you got a few of them. We need to work on that aim.";
        }
        else if (balloonsPopped < 8)
        {
            message = "Great shooting! You popped most of the balloons.";
        }
        else
        {
            message = "You nailed it! Your aim was on point, mercenary. I am impressed.";
        }

        dialogueManager.Add("Tutor Vega", "Secret Sanctum", message, 6f);

        dialogueManager.Add(
            "Tutor Vega",
            "Secret Sanctum",
            "Your last lesson is about exploiting enemy weakspots. You see that large balloon ahead? Aim for the yellow spot and pop the balloon!",
            8f
        );
    }

    public void EndThirdTrial(bool bossKilled)
    {
        score = GetScore();

        if (bossKilled)
        {
            score += 1;

            dialogueManager.Add(
                "Tutor Vega",
                "Secret Sanctum",
                "Nice shooting merchanery, you are quote the sharpshooter!",
                6f
            );
        }
        else
        {
            dialogueManager.Add(
                "Tutor Vega",
                "Secret Sanctum",
                "That's it, the wind has picked up and the balloon has flown away, perhaps next time.",
                8f
            );
        }

        if (score >= 5)
        {
            dialogueManager.Add(
                "Tutor Vega",
                "Secret Sanctum",
                "Outstanding work, mercenary! Your performance has been exceptional. We’re proud to welcome you into the ranks of Secret Sanctum. Congratulations!",
                10f
            );
        }
        else
        {
            dialogueManager.Add(
                "Tutor Vega",
                "Secret Sanctum",
                "Thank you for completing the evaluation, mercenary. We have reviewed your submission and found it suitable for the role we are looking for, Welcome to the ranks of secret Sanctum.",
                10f
             );
        }

        Invoke(nameof(EvaluationPassed), 21f);
    }

    private float GetScore()
    {
       return loopsPassed * 0.4f + balloonsPopped * 0.2f;
    }

    private void EvaluationFailed()
    {
        missionController.GameOver();
    }

    private void EvaluationPassed()
    {
        if (!missionStats.isCompleted)
        {
            // TODO:
            // badgeRewardBeginner.isOwned = true;

            GameManager.Instance.AddNewNotification(
                "Welcome in!",
                "Secret Sanctum",
                "TODO: Great to have you with us! Now that you have proven yourself let me officially invote you to our rank.We have granted you your furst badge of honor, please check your badges and wear it proudly.Once you are ready, meet me at the command sector and we will assign your next task for you.\n\nOh yeah, bt the way, the ammo and repairs are not covered by us, so you own us some money"
            );

            GameManager.Instance.AddNewNotification(
                "Instroduction to garage",
                "System",
                "TODO: This is your garage. Here you can manage your plane by editing the parts it is build of."
            );
        }

        if (score >= 5 && !badgeRewardElite.isOwned)
        {
            // TODO:
            // badgeRewardElite.isOwned = true;

            GameManager.Instance.AddNewNotification(
                "Exceptional work!",
                "Secret Sanctum",
                "TODO: Fantastic demonstartion of skill at the training area, we are proud to have you with us! Here take this token as a showcase of graditude."
            );
        }

        missionController.CompleteMission(Mathf.FloorToInt(score));
    }
}
