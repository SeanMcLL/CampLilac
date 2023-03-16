using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables for handling inputs
    public int horizontal;
    public float speed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Start horizontal at 0 every frame
        horizontal = 0;
        if (Input.GetKey(KeyCode.A)) {
            horizontal -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            horizontal += 1;
        }
    }
    //Fixed update for physics
    void FixedUpdate() {
        transform.Translate(horizontal*speed*Time.deltaTime,0f,0f);
    }
}
