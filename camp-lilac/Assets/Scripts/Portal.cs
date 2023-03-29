using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string destinationScene;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        CLS.Instance.SceneControlManager.DestinationScene = destinationScene;
        CLS.Instance.UIManager.blackout.SetTrigger("Blackout");
    }
}
