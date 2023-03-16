using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterActionState : StateMachineBehaviour
{
    GameManager gm;

    void OnStateEnter()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.AfterAction();
    }
}
