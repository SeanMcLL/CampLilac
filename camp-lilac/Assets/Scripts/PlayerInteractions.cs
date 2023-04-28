using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask enemyLayer;

    public GameObject currentInteractionTrigger;
    public bool hasBeenTriggered = false;
    public bool isTurn;
    public bool isColliding;
    public bool isShopMenu;
    public bool isDialogue;
    public GameObject player;
    public Camera cam;
    private GameManager gm;
    private UIManager um;
    private Combatant pc;
    public Animator animator;

    void Start() {
        //animator = GetComponent<Animator>();
        //animator.SetBool("isVisible", false);
        gm = CLS.Instance.GameManager;
        um = CLS.Instance.UIManager;
        pc = GetComponent<Combatant>();
        isColliding = false;
        
    }

    void Update(){
        if ((Input.GetKeyDown(KeyCode.E)) && (isColliding == true)){
                //Debug.Log("started interaction");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                cam.GetComponent<SmartCamera>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                //hasBeenTriggered = true;
                // goes to DialogueTrigger function 
                if (isDialogue == true){
                    currentInteractionTrigger.GetComponent<DialogueTrigger>().TriggerDialogue();
                }
                if (isShopMenu == true){
                    Debug.Log("is shop menu is true and e is pressed");
                    //activate shop menu
                    currentInteractionTrigger.GetComponent<WeaponShop>().OpenShop();
                }
            }
    }

    // triggers for when a player interacts with a game object
    void OnTriggerEnter2D(Collider2D col) {
        //interaction with an enemy
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            //not in combat
            if (!um.inCombat) {
                um.magicASP[0] = pc.spell1.w_name;
                um.magicASP[1] = pc.spell2.w_name;
                um.magicASP[2] = pc.spell3.w_name;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gm.StartCombat(col.gameObject);
            }
        }
        //interaction with an npc
        if (col.gameObject.layer == LayerMask.NameToLayer("NPC")) {
            animator = col.gameObject.transform.GetChild(0).GetComponent<Animator>();
            animator.SetBool("isVisible", true);
            isColliding = true;
            isDialogue = true;
            currentInteractionTrigger = col.gameObject;
        }
        
        //interaction with shop (upgrade sword or bow by blacksmith)
        if (col.gameObject.layer == LayerMask.NameToLayer("Shop")){
            animator = col.gameObject.transform.GetChild(0).GetComponent<Animator>();
            animator.SetBool("isVisible", true);
            isColliding = true;
            isShopMenu = true;
            currentInteractionTrigger = col.gameObject;

        }
        

        if (col.gameObject.layer == LayerMask.NameToLayer("Kill Plane"))
        {
            transform.position = CLS.Instance.SceneControlManager.CurrentSceneObjectsManager.PlayerOrigin.position;
            //Modify hp
            pc.hp -= 10;
            //Adjust health bar
            pc.healthBar.fillAmount = (float)pc.hp / (float)pc.maxHp;
        }
       
    }

    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("NPC")) {
            isColliding = false;
            isDialogue = false;
            currentInteractionTrigger = null;
            animator = col.gameObject.transform.GetChild(0).GetComponent<Animator>();
            animator.SetBool("isVisible", false);

        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Shop")) {
            isColliding = false;
            isShopMenu = false;
            currentInteractionTrigger = null;
            animator = col.gameObject.transform.GetChild(0).GetComponent<Animator>();
            animator.SetBool("isVisible", false);

        }

    }
}
