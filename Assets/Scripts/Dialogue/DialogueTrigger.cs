using UnityEngine;

namespace Remorse.Chat
{
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;
        public DialogueManager dialogueManager;

        public void TriggerDialogue()
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }

}