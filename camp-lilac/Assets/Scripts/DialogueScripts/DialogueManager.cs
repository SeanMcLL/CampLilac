using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject player;
    public Camera cam;
    public AudioManager am;

    public Animator animator;

    public bool inDialogue;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        // set that we are in a dialogue event so we set the inDialogue bool to be true and generate a new queue of 
        // sentences for the entity you are in a dialogue with
        inDialogue = false;
        sentences = new Queue<string>();
        am = CLS.Instance.AudioManager;
    }

    void Update()
    {
        // every time you hit enter trigger DisplayNextSentence
        if (Input.GetKeyDown(KeyCode.Return) && inDialogue == true)
        {
            Debug.Log("Display next sentence");
            am.sfxSource.clip = am.selectSfx;
            am.sfxSource.Play();
            DisplayNextSentence();
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        inDialogue = true;
        //Debug.Log("started Dialogue");
        // set dialogue animator to true and bring dialogue box into view
        animator.SetBool("isOpen", true);
        // set name of person talking in dialogue box
        nameText.text = dialogue.name;
        // clear any sentences from previous interactions
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            // populate dialogue box with current sentence
            sentences.Enqueue(sentence);
        }
        // move on to next sentences
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //Debug.Log(sentences.Count);
        // check if there are any sentences left to display
        if (sentences.Count == 0)
        {
            // if
            // check if some other interaction needs to happen (menu, cutscene, scene load, etc.)

            //else
            // if there are no more sentences to display we allow the player and camera to move again and end the dialogue
            EndDialogue();
            cam.GetComponent<SmartCamera>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            return;
        }
        // take current sentence out of the dialogue box and stop the current coroutine
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // IEnumerator populates the dialogue box one letter at a time to create the typing look of the dialogue
    // becoming visible
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        // set inDialogue to false and have the animator take the dialogue box out of frame
        Debug.Log("Dialogue Ended");
        inDialogue = false;
        animator.SetBool("isOpen", false);
    }
}
