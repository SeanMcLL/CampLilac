using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Combatant : MonoBehaviour
{
    public string combatantName;
    //Game Manager
    private GameManager gm;
    private UIManager um;
    private AudioManager am;

    //UI References
    public Image healthBar;
    public GameObject attackIcon;
    public TextMeshProUGUI damageNumber;
    public TextMeshProUGUI statusText;

    //Hit Points
    public int hp;
    public int maxHp;

    //Mana Points
    public int mp;
    public int maxMp;

    // Skill Points
    public int upgradePoints;
    

    //Weakness Enum
    public enum Weakness
    {
        melee,
        ranged,
        fire,
        ice,
        lightning
    }

    //Resistance Enum
    public enum Resistance
    {
        melee,
        ranged,
        fire,
        ice,
        lightning
    }

    //Combatant Primary Stats
    public int con;
    public int str;
    public int dex;
    public int wis;
    public int luc;

    //Combatant Calculated Stats
    public int defense;
    public float evasion;
    public float critchance;

    //Combatant Weapons
    public Weapon mWeapon;
    public Weapon rWeapon;

    //Combatant Spells
    public Weapon spell1;
    public Weapon spell2;
    public Weapon spell3;

    //Bool to check if players are alive and can take their turn
    public bool isAlive;

    void Start() {
        gm = CLS.Instance.GameManager;
        um = CLS.Instance.UIManager;
        am = CLS.Instance.AudioManager;

        defense = 3+str;
        evasion = 0+(luc*0.5f);
        critchance = 5+(luc*0.25f);

        isAlive = true;
        
    }

    //Methods
    public void TakeTurn() {
        if (isAlive == true){
            if (gameObject.tag == "Player") {
                //Enable menu controls
                //Display the battle actions UI
                um.EnableBattleActionUIPanel();
            } else {
                ChooseAction(GameManager.ActionType.attack, AIChooseAttackSlot());
            }
        }
        else {
            um.DisableBattleActionUIPanel();
        }
        
    }

    public void ChooseAction(GameManager.ActionType actionType, int slot) {
        Combatant target = gm.findTargets(this)[0];
        if (gameObject.tag == "Player") {
            um.ChangePanel(um.mainASP);
            um.DisableBattleActionUIPanel();
            //update combat machine
            Animator combatMachine = gm.GetComponent<Animator>();
            combatMachine.SetTrigger("ChoseTarget");
            combatMachine.SetTrigger("ChoseAction");
        }
        if (actionType == GameManager.ActionType.attack)
        {
            Sprite attackIconSprite = null;
            switch (slot)
            {
                case 0:
                    attackIconSprite = mWeapon.attackIconSprite;
                    break;
                case 1:
                    attackIconSprite = rWeapon.attackIconSprite;
                    break;
                case 2:
                    attackIconSprite = spell1.attackIconSprite;
                    break;
                case 3:
                    attackIconSprite = spell2.attackIconSprite;
                    break;
                case 4:
                    attackIconSprite = spell3.attackIconSprite;
                    break;
            }
            attackIcon.GetComponent<SpriteRenderer>().sprite = attackIconSprite;
        }
        if (actionType == GameManager.ActionType.attack && slot==0) {
            float startX = transform.position.x;
            float offset = (IsTeamGood())?-1f:1f;
            Sequence meleeDashSeq = DOTween.Sequence();
            meleeDashSeq.Append(transform.DOMoveX(target.transform.localPosition.x+offset, 0.25f))
                .PrependInterval(1f)
                .Append(transform.DOMoveX(startX, 0.5f));
        }
        attackIcon.SetActive(true);
        am.sfxSource.clip = am.hitSfx;
        am.sfxSource.Play();
        gm.ReceiveAction(actionType, slot, target);
    }

    public float ComputeDamageApplied(float damage) {
        float d = damage;
        //Apply modifiers to damage value

        //Roll for critical hit
        if (CritCheck()) {d*=2;} //Do Double damage on critical hit
        return d;
    }
    public float ComputeDamageRecieved(float damage) {
        //Apply modifiers to damage value
        //Debug.Log("Defense="+defense);
        float d = (damage-(defense/2))/2;
        return d;
    }
    public void TakeDamage(float damage) {
        //Check for evade
        if (EvasionCheck())
        {
            UpdateStatusText("Dodged!");
            return;
        }
        //Compute the damage received after defenseive modifiers
        float adjDamage = ComputeDamageRecieved(damage);
        //Round adjDamage to int
        int intDamage = Mathf.RoundToInt(adjDamage);
        if (intDamage == 0) {
            intDamage = (Random.value>0.5)?1:0;
        }
        //Modify hp
        hp -= intDamage;
        //Adjust health bar
        healthBar.fillAmount = (float)hp/(float)maxHp;
        //Apply damage number
        damageNumber.text = intDamage.ToString();
        //Define damage number animation
        damageNumber.GetComponent<Animator>().SetTrigger("ShowDamageNumber");
        //Check die
        if (hp <= 0) {
            Die();
        }
    }
    public void Die() {
        isAlive = false;
        List<Combatant> team = gm.findTeam(this);
        team.Remove(this);
        gm.turnOrder.Remove(this);
        /*
        if (gameObject.tag == "Player") {
            um.DisableBattleActionUIPanel();
            Debug.Log("panel should be disabled");
            um.ShowDeathScreen();
            Debug.Log("death screen open");
        }
        */
        if (gameObject.tag == "Enemy"){
            Debug.Log("Enemy Killed");
            gameObject.SetActive(false);
            //um.ShowVictoryScreen();
        }
    }

    public bool CritCheck() {
        if (Random.Range(0f, 1f) < critchance/100) {
            Debug.Log("Critical Hit!");
            UpdateStatusText("Crit!");
            return true;
        }
        return false;
    }

    public bool EvasionCheck()
    {
        if (Random.Range(0f, 1f) < evasion / 100)
        {
            Debug.Log("Dodged Hit!");
            return true;
        }
        return false;
    }

    public bool IsTeamGood() {
        foreach (Combatant c in gm.teamGood) {
            if (c == this) {
                return true;
            }
        }
        return false;
    }

    public void UpdateStatusText(string text)
    {
        Debug.Log(name);
        statusText.text = text;
        statusText.GetComponent<Animator>().SetTrigger("ShowStatusText");
    }

    int AIChooseAttackSlot()
    {
        List<int> possibleAttackSlots = new();
        if (mWeapon != null)
        {
            possibleAttackSlots.Add(0);
        } if (rWeapon != null)
        {
            possibleAttackSlots.Add(1);
        } if (spell1 != null)
        {
            possibleAttackSlots.Add(2);
        } if (spell2 != null)
        {
            possibleAttackSlots.Add(3);
        } if (spell3 != null)
        {
            possibleAttackSlots.Add(4);
        }
        return possibleAttackSlots[Random.Range(0, possibleAttackSlots.Count)];
    }
}

