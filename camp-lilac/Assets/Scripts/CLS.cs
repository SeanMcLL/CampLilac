using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CLS : MonoBehaviour
{
    public static CLS Instance { get; private set; }
    public GameManager GameManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public SceneControlManager SceneControlManager { get; private set; }
    public GameObject Player { get; private set; }

    public CinemachineVirtualCamera CMVcam { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        GameManager = GetComponentInChildren<GameManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        UIManager = GetComponentInChildren<UIManager>();
        DialogueManager = GetComponentInChildren<DialogueManager>();
        SceneControlManager = GetComponentInChildren<SceneControlManager>();
        Player = GameObject.Find("Player");
        CMVcam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
