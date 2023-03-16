using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "camp-lilac/Weapon", order = 0)]

public class Weapon : ScriptableObject
{
    
    //Item info
    public string w_name;
    public float value;
    public int rarity;
    public Sprite attackIconSprite;

    //Stats
    public float damage;
    public float accuracy;
    public bool isCounterable;

    //Public Methods
    //------------------------------------------------

    //Compares random roll to accuracy of weapon, returns whethere attack hits or not
    public bool HitCheck() {
        if (Random.Range(0f, 1f) < accuracy) {return true;}
        return false;
    }

}
[CreateAssetMenu(fileName = "Spell", menuName = "camp-lilac/Spell", order = 1)]
public class Spell : Weapon

{
    public int manaCost;
}
