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
        PlayerOrigin = GameObject.Find("PlayerOrigin").transform;
        LeftCamBorder = GameObject.Find("LeftCamBorder").transform;
        RightCamBorder = GameObject.Find("RightCamBorder").transform;

        CLS.Instance.SceneControlManager.CurrentSceneObjectsManager = this;
        SmartCamera smartCamera = Camera.main.GetComponent<SmartCamera>();
        smartCamera.leftLevelWall = LeftCamBorder;
        smartCamera.rightLevelWall = RightCamBorder;
    }
}
