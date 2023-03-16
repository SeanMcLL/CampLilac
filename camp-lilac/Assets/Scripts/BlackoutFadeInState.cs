using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackoutFadeInState : StateMachineBehaviour
{
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public void OnStateExit(Animator animator)
    {
        //Load the destination scene additively
        SceneManager.LoadSceneAsync(CLS.Instance.GameManager.DestinationScene, LoadSceneMode.Additive);

        //Unload the current scene
        SceneManager.UnloadSceneAsync(CLS.Instance.GameManager.CurrentScene);

        //Change Current Scene in Game Manager
        CLS.Instance.GameManager.CurrentScene = CLS.Instance.GameManager.DestinationScene;
    }
}
