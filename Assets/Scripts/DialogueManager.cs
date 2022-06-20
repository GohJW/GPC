using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    public GameObject cursor;

    public Animator animator;

    private Queue<string> names;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();

        StartDialogue(dialogue);
    }

    public void StartDialogue (Dialogue dialogue)
    {
        cursor.SetActive(false);

        animator.SetBool("IsOpen", true);

        names.Clear();
        sentences.Clear();

        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextName();
        DisplayNextSentence();
    }

    public void DisplayNextName()
    {
        if (names.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = names.Dequeue();
        nameText.text = name;
    }
    public void DisplayNextSentence () 
    {
       if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }



    void EndDialogue() // Hide dialogue Box
    {
        animator.SetBool("IsOpen", false);
        cursor.SetActive(true);
    }
}
