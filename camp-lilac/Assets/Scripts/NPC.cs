using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public LayerMask npcLayer;

    public bool hasBeenTriggered = false;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("NPC") && hasBeenTriggered == false) {
            //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasBeenTriggered = true;
            Debug.Log("open dialogue");
        }
    }
}
