using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lilacs : MonoBehaviour
{
    //References for swapping the sprite
    public SpriteRenderer r;
    public Sprite normalLilac;
    public Sprite crushedLilac;

    private AudioSource asource;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        asource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            r.sprite = crushedLilac;
            asource.Play();
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            r.sprite = normalLilac;
        }
    }
}
