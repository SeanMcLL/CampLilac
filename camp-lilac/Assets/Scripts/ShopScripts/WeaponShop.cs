using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponShop : MonoBehaviour
{

    public TextMeshProUGUI UpgradePointsValue;
    public TextMeshProUGUI WeaponPointsText;
    public Weapon sword;
    public Weapon bow;
    public Combatant player;
    public bool swordSelected;
    public bool bowSelected;
    public bool inShop;


    // Start is called before the first frame update
    void Start()
    {
        player = CLS.Instance.Player.GetComponent<Combatant>();
        // temp manual set of sword selected
        swordSelected = true;
        
    }

    public void OpenShop()
    {
        Debug.Log("shop should open");
        CLS.Instance.UIManager.EnableWeaponCanvas();
        inShop = true;
    }

    public void CloseShop()
    {
        CLS.Instance.UIManager.DisableWeaponCanvas();
        player.GetComponent<PlayerMovement>().enabled = true;
        inShop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inShop)
        {
            if (swordSelected)
            {
                if (player.upgradePoints > 0)
                {
                    // increase stats for weapon
                    if(Input.GetKeyDown(KeyCode.A))
                    {
                        player.mWeapon.damage -= 1;
                        player.upgradePoints += 1;
                        CLS.Instance.UIManager.UpdateWeaponCanvas();
                    }
                    // decrease stats for weapon
                    if(Input.GetKeyDown(KeyCode.D))
                    {
                        player.mWeapon.damage += 1;
                        player.upgradePoints -= 1;
                        CLS.Instance.UIManager.UpdateWeaponCanvas();        
                    }
                }
            }

            if (bowSelected)
            {
                if (player.upgradePoints > 0)
                {
                    // increase stats for weapon
                    if(Input.GetKeyDown(KeyCode.A))
                    {
                        player.rWeapon.damage -= 1;
                        player.upgradePoints += 1;
                        CLS.Instance.UIManager.UpdateWeaponCanvas();
                    }
                    // decrease stats for weapon
                    if(Input.GetKeyDown(KeyCode.D))
                    {
                        player.rWeapon.damage += 1;
                        player.upgradePoints -= 1;
                        CLS.Instance.UIManager.UpdateWeaponCanvas();       
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
                CloseShop();
            }
        }    
    }
}
