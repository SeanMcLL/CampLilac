using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager gm;
    public AudioManager am;
    public Combatant player;
    public GameObject battleActionUIPanel;
    public bool inCombat;
    public Animator endCombatPanelAnimator;

    public void EnableBattleActionUIPanel() {
        battleActionUIPanel.SetActive(true);
        selectedActionText = 1;
    }

    public void DisableBattleActionUIPanel() {
        battleActionUIPanel.SetActive(false);
    }

    //Battle Action Chosing
    public int selectedActionText;
    public float actionSelectorOffset;
    public Image actionSelector;
    public TextMeshProUGUI actionText1;
    public TextMeshProUGUI actionText2;
    public TextMeshProUGUI actionText3;
    public TextMeshProUGUI actionText4;
    public GameObject endCombatPanel;
    public TextMeshProUGUI endCombatText;
    public Vector3 endCombatPanelRestPos;
    //Battle UI
    public GameObject battleUI;
    public TextMeshProUGUI leftName;
    public TextMeshProUGUI rightName;
    public GameObject deathScreen;
    public Animator blackout;

    //Combat UI
    public Image leftHealthBar;
    public Image rightHealthBar;

    void Start() {
        //Debug.Log("Start was called");

        gm = CLS.Instance.GameManager;
        am = CLS.Instance.AudioManager;
        player = GameObject.Find("Player").GetComponent<Combatant>();
        endCombatPanelAnimator = GameObject.Find("EndCombatPanel").GetComponent<Animator>();
        currentASP = mainASP;
    }

    void Update() {
        //functinality to move selector when keys are pressed in combat
        if (Input.GetKeyDown(KeyCode.A) && inCombat == true) {
            selectedActionText -= 1;
            CheckActionIndexBounds();
            MoveActionSelector();
        } else if (Input.GetKeyDown(KeyCode.D) && inCombat == true) {
            selectedActionText += 1;
            CheckActionIndexBounds();
            MoveActionSelector();
        } else if (Input.GetKeyDown(KeyCode.W) && inCombat == true) {
            selectedActionText -= 2;
            CheckActionIndexBounds();
            MoveActionSelector();
        } else if (Input.GetKeyDown(KeyCode.S) && inCombat == true) {
            selectedActionText += 2;
            CheckActionIndexBounds();
            MoveActionSelector();
        }
        if (Input.GetKeyDown(KeyCode.Return) && inCombat == true) {
            SelectAction();
        }
    }

    void CheckActionIndexBounds() {
        if (selectedActionText < 1) {
            selectedActionText = 1;
        } else if (selectedActionText > 4) {
            selectedActionText = 4;
        }
    }

    void MoveActionSelector() {
        am.sfxSource.clip = am.pointerSfx;
        am.sfxSource.Play();
        Vector2 pos = Vector2.zero;
        switch(selectedActionText) {
            case 1:
                pos = actionText1.transform.position;
                break;
            case 2:
                pos = actionText2.transform.position;
                break;
            case 3:
                pos = actionText3.transform.position;
                break;
            case 4:
                pos = actionText4.transform.position;
                break;
        }
        actionSelector.transform.position = new Vector2(pos.x-actionSelectorOffset,pos.y);
    }


    public string[] mainASP = new string[]{"Attack", "Magic", "Item", "Flee"};
    public string[] attackASP = new string[]{"Melee", "Ranged", "", "Back"};
    public string[] magicASP = new string[]{"Magic1", "Magic2", "Magic3", "Back"};
    public string[] itemASP = new string[]{"Item1", "Item2", "Item3", "Back"};
    string[] currentASP;
    

    void SelectAction() {
        am.sfxSource.clip = am.selectSfx;
        am.sfxSource.Play();
        if (currentASP == mainASP) {
            switch(selectedActionText) {
                case 1:
                    ChangePanel(attackASP);
                    break;
                case 2:
                    ChangePanel(magicASP);
                    break;
                case 3:
                    ChangePanel(itemASP);
                    break;
                case 4:
                    Debug.Log("Flee");
                    break;
            }
        }
        else if (currentASP == attackASP) {
            switch(selectedActionText) {
                case 1:
                    player.ChooseAction(GameManager.ActionType.attack, 0);
                    break;
                case 2:
                    player.ChooseAction(GameManager.ActionType.attack, 1);
                    break;
                case 3:
                    break;
                case 4:
                    ChangePanel(mainASP);
                    break;
            }
        }
        else if (currentASP == magicASP) {
            switch(selectedActionText) {
                case 1:
                    player.ChooseAction(GameManager.ActionType.attack, 2);
                    break;
                case 2:
                    player.ChooseAction(GameManager.ActionType.attack, 3);
                    break;
                case 3:
                    player.ChooseAction(GameManager.ActionType.attack, 4);
                    break;
                case 4:
                    ChangePanel(mainASP);
                    break;
            }
        }
        else if (currentASP == itemASP) {
            switch(selectedActionText) {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    ChangePanel(mainASP);
                    break;
            }
        }
    }
    
    public void ChangePanel(string[] panel) {
        currentASP = panel;
        actionText1.text = panel[0];
        actionText2.text = panel[1];
        actionText3.text = panel[2];
        actionText4.text = panel[3];
        selectedActionText = 1;
        MoveActionSelector();
    }

    public void ShowDeathScreen() {
        //DisableBattleActionUIPanel();
        inCombat = false;
        deathScreen.SetActive(true);
        
    }
    public void EndCombatPanel(bool didWin) {
        string endText = (didWin)?("You defeated the enemies!"):("Your party has been defeated!");
        endCombatText.text = endText;
        endCombatPanelRestPos = endCombatPanel.GetComponent<RectTransform>().position;
        endCombatPanelAnimator.SetBool("isOpen", true);
        /*
        Sequence endCombatPanelRise = DOTween.Sequence();
        endCombatPanelRise.Append(endCombatPanel.GetComponent<RectTransform>().DOMoveY(-400f, 1f));
        */
    }
    public void HideEndCombatPanel()
    {
        endCombatPanelAnimator.SetBool("isOpen", false);
    }
}
