using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponShop : MonoBehaviour
{

    public TextMeshProUGUI UpgradePointsValue;
    public TextMeshProUGUI SwordDMGText;
    public TextMeshProUGUI BowDMGText;
    public Weapon sword;
    public Weapon bow;
    public Combatant player;
    public bool swordSelected;
    public bool bowSelected;

    // Start is called before the first frame update
    void Start()
    {
        UpgradePointsValue.text = player.upgradePoints.ToString();
        SwordDMGText.text = sword.damage.ToString();
        BowDMGText.text = bow.damage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        while (player.upgradePoints > 0){
            if(Input.GetKeyDown(KeyCode.O)){
                sword.damage += 1;
                player.upgradePoints -= 1;
                UpgradePointsValue.text = player.upgradePoints.ToString();
                SwordDMGText.text = sword.damage.ToString();
        }
            if(Input.GetKeyDown(KeyCode.P)){
                bow.damage += 1;
                player.upgradePoints -= 1;
                UpgradePointsValue.text = player.upgradePoints.ToString();
                BowDMGText.text = bow.damage.ToString();
        }
        
        }
        
    }
}
