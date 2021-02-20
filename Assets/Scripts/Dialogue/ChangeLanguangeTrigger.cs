using UnityEngine;
using Remorse.Localize;

public class ChangeLanguangeTrigger : MonoBehaviour
{
    public string languangeId;

    public void TriggerChange()
    {
        Localization.ChangeLanguange(languangeId);

        /*Dialogue dialogue = FindObjectOfType<DialogueTrigger>().dialogue;
        int sentencesLength = dialogue.sentences.Length;

        for (int i = 0; i < sentencesLength; i++)
        {
            Debug.Log(dialogue.sentences[i].value);
        }*/

        /*var dialogueManagers = FindObjectsOfType<DialogueManager>();
        foreach (var dialogueManager in dialogueManagers)
        {
            dialogueManager.StartDialogue();
        }*/
    }
}
