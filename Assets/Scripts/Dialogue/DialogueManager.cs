using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Remorse.Chat
{
    public class DialogueManager : MonoBehaviour
    {
        public Text nameText;
        public Text dialogueText;

        string[] sentencesValues;

        private Queue<string> sentences;

        // Start is called before the first frame update
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            nameText.text = dialogue.name.value;

            int sentencesLength = dialogue.sentences.Count;
            sentencesValues = new string[sentencesLength];

            for (int i = 0; i < sentencesLength; i++)
            {
                sentencesValues[i] = dialogue.sentences[i].value;
            }

            sentences.Clear();

            foreach (string sentence in sentencesValues)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentences();
        }

        public void DisplayNextSentences()
        {
            if(sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        public void EndDialogue()
        {
            Debug.Log("End Dialogue");
        }
    }

}
