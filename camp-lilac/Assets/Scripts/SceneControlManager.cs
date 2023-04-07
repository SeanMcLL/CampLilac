using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public string CurrentScene;
    public string DestinationScene;
    public SceneObjectsManager CurrentSceneObjectsManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Persistent")
        {
            CLS.Instance.Player.transform.position = CurrentSceneObjectsManager.PlayerOrigin.position;
            CLS.Instance.Player.GetComponent<PlayerMovement>().enabled = true;
        }
    }

}
