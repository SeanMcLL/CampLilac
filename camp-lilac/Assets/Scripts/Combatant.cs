using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Combatant : MonoBehaviour
{
    //Game Manager
    private GameManager gm;
    private UIManager um;
    private AudioManager am;

    //UI References
    public Image healthBar;
    public GameObject attackIcon;
    public TextMeshProUGUI damageNumber;

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
                ChooseAction(GameManager.ActionType.attack, 0);
            }
        }
        else {
            um.DisableBattleActionUIPanel();
        }
        
    }

    public void ChooseAction(GameManager.ActionType actionType, int slot) {
        Debug.Log(gameObject.name);
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
            attackIcon.GetComponent<SpriteRenderer>().sprite = slot==0?mWeapon.attackIconSprite:rWeapon.attackIconSprite;
        }
        else if (actionType == GameManager.ActionType.spell)
        {
            if (slot == 0)
            {
                attackIcon.GetComponent<SpriteRenderer>().sprite = spell1.attackIconSprite;
            }
            else if (slot == 1)
            {
                attackIcon.GetComponent<SpriteRenderer>().sprite = spell2.attackIconSprite;
            }
            else if (slot == 2)
            {
                attackIcon.GetComponent<SpriteRenderer>().sprite = spell3.attackIconSprite;
            }
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
        //Compute the damage received after defenseive modifiers
        float adjDamage = ComputeDamageRecieved(damage);
        //Round adjDamage to int
        int intDamage = Mathf.RoundToInt(adjDamage);
        if (intDamage == 0) {
            intDamage = (Random.value>0.5)?1:0;
            Debug.Log(intDamage);
        }
        //Modify hp
        hp -= intDamage;
        //Adjust health bar
        healthBar.fillAmount = (float)hp/(float)maxHp;
        //Apply damage number
        damageNumber.text = intDamage.ToString();
        //Define damage number animation
        float startYPos = damageNumber.transform.position.y;
        Sequence damageNumberFade = DOTween.Sequence();
        damageNumberFade.Append(damageNumber.DOFade(1, 0.1f))
            .PrependInterval(1)
            .Append(damageNumber.DOFade(0, 2));
        Sequence damageNumberFloat = DOTween.Sequence();
        damageNumberFloat.Append(damageNumber.transform.DOMoveY(startYPos+1f,2))
            .PrependInterval(1)
            .Append(damageNumber.transform.DOMoveY(startYPos,0.25f));
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
}
