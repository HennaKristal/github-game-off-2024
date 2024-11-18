using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI factionText;
    [SerializeField] private TextMeshProUGUI messageText;

    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    private Animator animator;
    private bool isDisplaying = false;

    private class Dialogue
    {
        public string Title;
        public string Faction;
        public string Message;
        public float Duration;

        public Dialogue(string title, string faction, string message, float duration)
        {
            Title = title;
            Faction = faction;
            Message = message;
            Duration = duration;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDisplaying && dialogueQueue.Count > 0)
        {
            StartCoroutine(DisplayDialogue(dialogueQueue.Dequeue()));
        }
    }

    public void Add(string title, string faction, string message, float duration)
    {
        dialogueQueue.Enqueue(new Dialogue(title, faction, message, duration));
    }

    private IEnumerator DisplayDialogue(Dialogue dialogue)
    {
        isDisplaying = true;
        animator.SetBool("isActive", true);

        yield return new WaitForSeconds(0.5f);

        titleText.text = dialogue.Title;
        factionText.text = dialogue.Faction;
        messageText.text = dialogue.Message;

        yield return new WaitForSeconds(dialogue.Duration);

        animator.SetBool("isActive", false);

        yield return new WaitForSeconds(1f);

        isDisplaying = false;
    }
}
