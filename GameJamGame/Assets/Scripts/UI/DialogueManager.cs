using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManagerTMP : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    [SerializeField] private AudioSource typingAudioSource;
    [SerializeField] private AudioClip typingClip;
    public static bool IsDialogueActive { get; private set; } = false;
    public static event Action OnDialogueEnded;

    private Queue<string> sentences;
    private bool isTyping = false;

    // store the sentence currently being typed
    private string currentSentence;

    private void Awake()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        IsDialogueActive = true;
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping) 
        {
            // If already typing, skip to full sentence instantly
            SkipTyping();
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingAudioSource != null && typingClip != null)
        {
            typingAudioSource.clip = typingClip;
        typingAudioSource.loop = true;
        typingAudioSource.Play();
        }

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (typingAudioSource != null)
        {
            typingAudioSource.Stop();
            typingAudioSource.loop = false;
        }

        isTyping = false;
    }

    private void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.text = currentSentence;

        if (typingAudioSource != null)
        {
            typingAudioSource.Stop();
            typingAudioSource.loop = false;
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueActive = false;
        OnDialogueEnded?.Invoke();
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.anyKeyDown)
        {
            DisplayNextSentence();
        }
    }
}
