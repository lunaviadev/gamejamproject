using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] startDialogue;

    private void Start()
    {
        DialogueManagerTMP dialogueManager = FindFirstObjectByType<DialogueManagerTMP>();
        if (dialogueManager != null && startDialogue.Length > 0)
        {
            dialogueManager.StartDialogue(startDialogue);
        }
    }
}
