using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public string CurrentScene;
    public string DestinationScene;
    public SceneObjectsManager CurrentSceneObjectsManager;

    public void InitScene()
    {
        CLS.Instance.Player.transform.position = CurrentSceneObjectsManager.PlayerOrigin.position;
        CLS.Instance.Player.GetComponent<PlayerMovement>().enabled = true;
    }

}
