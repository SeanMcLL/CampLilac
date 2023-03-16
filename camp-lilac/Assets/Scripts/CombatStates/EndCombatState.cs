using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCombatState : StateMachineBehaviour
{
    GameManager gm;
    void OnStateExit()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.PlayerReturn();
    }
}