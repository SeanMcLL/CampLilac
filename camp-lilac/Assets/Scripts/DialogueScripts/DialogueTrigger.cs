using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    // called by PlayerInteractions.cs when player comes into contact with
    // an entity that has a dialogue sequence
    public void TriggerDialogue ()
    {
        // start dialogue with the entity by calling StartDialogue with the
        // entities dialogue as a parameter
        // goes to DialogueManager to StartDialogue
        //Debug.Log("Triggered Dialogue");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
