using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChooseState : StateMachineBehaviour
{
    GameManager gm;

    void OnStateEnter()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.StartTurn();
    }
}
