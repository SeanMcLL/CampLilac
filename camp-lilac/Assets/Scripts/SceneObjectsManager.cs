using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour
{
    public Transform PlayerOrigin;
    public Transform LeftCamBorder;
    public Transform RightCamBorder;

    void Awake()
    {
        PlayerOrigin = transform.GetChild(0);
        LeftCamBorder = transform.GetChild(1);
        RightCamBorder = transform.GetChild(2);

        CLS.Instance.SceneControlManager.CurrentSceneObjectsManager = this;
        CLS.Instance.SceneControlManager.InitScene();
        SmartCamera smartCamera = Camera.main.GetComponent<SmartCamera>();
    }
}
