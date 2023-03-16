using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum ActionType {
        attack,
        spell,
        item
    }
    public struct ActionInfo
    {
        public ActionType ActionType;
        public int Slot;
        public Combatant Target;
        public ActionInfo(ActionType actionType, int slot, Combatant target)
        {
            ActionType = actionType;
            Slot = slot;
            Target = target;
        }
    }
    private ActionInfo storedActionInfo;
    public UIManager um;
    public AudioManager am;
    public Animator combatMachine;
    public GameObject player; 
    public Combatant enemy;
    private Vector3 playerPlatformPosition;
    public Transform battleZonePoint;
    public Transform playerZonePoint;
    public Transform enemyZonePoint;
    public Camera cam;
    public string CurrentScene;
    public string DestinationScene;

    //Stored Damage and Target
    private float storedDamage;
    private float storedTarget;

    void Start() {
        //Set variable references
        um = CLS.Instance.UIManager;
        am = CLS.Instance.AudioManager;
        combatMachine = GetComponent<Animator>();
        player = GameObject.Find("Player");
        battleZonePoint = GameObject.Find("BattleZonePoint").transform;
        playerZonePoint = GameObject.Find("PlayerZonePoint").transform;
        enemyZonePoint = GameObject.Find("EnemyZonePoint").transform;
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    public void StartCombat(GameObject e) {
        //start combat music
        um.inCombat = true;
        am.musicSource.clip = am.battleTheme;
        am.musicSource.Play();
        //Set persistent enemy variable
        enemy = e.GetComponent<Combatant>();
        //Disable the smart camera and player movement
        cam.GetComponent<SmartCamera>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        //Teleport camera, player, and enemy to battle zone
        playerPlatformPosition = player.transform.position;
        cam.transform.position = battleZonePoint.position;
        player.transform.position = playerZonePoint.position;
        e.transform.position = enemyZonePoint.position;
        //Form turn order
        teamGood.Add(player.GetComponent<Combatant>());
        teamBad.Add(enemy);
        FormTurnOrder();
        //enable combat ui
        um.battleUI.SetActive(true);
        Combatant turnTaker = turnOrder[currentTurnOrder];
        //update combat machine
        bool isPlayerTurn = (turnTaker.gameObject.tag == "Player");
        combatMachine.SetBool("isPlayerTurn", isPlayerTurn);
        //update the combat machine
        combatMachine.SetTrigger("StartCombat");
    }

    public List<Combatant> turnOrder = new List<Combatant>();
    public int currentTurnOrder = 0;
    public List<Combatant> teamGood = new List<Combatant>();
    public List<Combatant> teamBad = new List<Combatant>();

    public List<Combatant> findTeam(Combatant combatant) {
        foreach (Combatant teamMember in teamGood) {
            if (teamMember==combatant) {
                return teamGood;
            }
        }
        return teamBad;
    }

    public List<Combatant> findTargets(Combatant combatant) {
        return (findTeam(combatant)==teamGood?teamBad:teamGood);
    }

    void FormTurnOrder() {
        //Re define the turn order list
        turnOrder = new List<Combatant>();
        //Add player's members
        turnOrder.Add(player.GetComponent<Combatant>());
        player.GetComponent<Combatant>().healthBar = um.leftHealthBar;
        turnOrder.Add(enemy);
        enemy.healthBar = um.rightHealthBar;
    }

    void UpdateTurnTaker() {
        currentTurnOrder++;
        if (currentTurnOrder>=turnOrder.Count) {
            currentTurnOrder = 0;
        }
    }

    //Recieve action from the combatant
    public void ReceiveAction(ActionType actionType, int slot, Combatant target) {
        Debug.Log("Recieved Action! - " + actionType);
        //Cache turn taker
        Combatant actionTaker = turnOrder[currentTurnOrder];

        storedActionInfo = new ActionInfo(actionType, slot, target);
        StartCoroutine(DelayNextTurn(actionTaker));
    }
    IEnumerator DelayNextTurn(Combatant actionTaker)
    {
        yield return new WaitForSeconds(2);
        actionTaker.attackIcon.SetActive(false);
        UpdateTurnTaker();
        Combatant turnTaker = turnOrder[currentTurnOrder];
        //update combat machine
        bool isPlayerTurn = (turnTaker.gameObject.tag == "Player");
        combatMachine.SetBool("isPlayerTurn", isPlayerTurn);
        Debug.Log(teamGood);
        Debug.Log(teamBad);
        if (findTargets(turnTaker).Count == 0) {
            Debug.Log("end combat");
            if (findTeam(turnTaker) == teamGood) {
                um.EndCombatPanel(true);
            } else {
                um.EndCombatPanel(false);
            }
            EndCombat();
        }   
    }

    public void StartTurn()
    {
        Combatant turnTaker = turnOrder[currentTurnOrder];
        turnTaker.TakeTurn();
    }

    public void AfterAction()
    {
        ActionType actionType = storedActionInfo.ActionType;
        int slot = storedActionInfo.Slot;
        Combatant target = storedActionInfo.Target;
        //Cache turn taker
        Combatant actionTaker = turnOrder[currentTurnOrder];
        switch (actionType)
        {
            //On selecting the attack action
            case ActionType.attack:
                Weapon weapon = slot == 0 ? actionTaker.mWeapon : actionTaker.rWeapon;
                if (weapon.HitCheck())
                {
                    float damage = actionTaker.ComputeDamageApplied(weapon.damage);
                    target.TakeDamage(damage);
                }
                break;

            case ActionType.spell:
                Weapon spell = actionTaker.spell1;
                switch (slot)
                {
                    case 0:
                        spell = actionTaker.spell1;
                        break;
                    case 1:
                        spell = actionTaker.spell2;
                        break;
                    case 2:
                        spell = actionTaker.spell3;
                        break;
                }
                if (spell.HitCheck())
                {
                    float damage = actionTaker.ComputeDamageApplied(spell.damage);
                    target.TakeDamage(damage);
                }
                break;
        }
    }

    public void EndCombat() {
        //update combat machine
        combatMachine.SetBool("isEndCombat", true);
    }
    

    public void PlayerReturn()
    {
        player.transform.position = playerPlatformPosition;
        um.battleUI.SetActive(false);
        cam.GetComponent<SmartCamera>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        um.HideEndCombatPanel();
    }

}
