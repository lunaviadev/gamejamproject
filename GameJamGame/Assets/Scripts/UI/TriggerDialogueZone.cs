using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerDialogueZone : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            DialogueManagerTMP dm = GameObject.Find("DialogueManager").GetComponent<DialogueManagerTMP>();
            if (dm != null)
            {
                DialogueManagerTMP.OnDialogueEnded += OnDialogueFinished;
                dm.StartDialogue(dialogueLines);
            }
        }
    }

    private void OnDialogueFinished()
    {
        DialogueManagerTMP.OnDialogueEnded -= OnDialogueFinished;
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
