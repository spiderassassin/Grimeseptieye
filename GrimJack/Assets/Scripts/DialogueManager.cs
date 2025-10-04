using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public TMP_Text dialogueText;
    // Start is called before the first frame update
    void Start() { 

        sentences = new Queue<string>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {

    }
}
