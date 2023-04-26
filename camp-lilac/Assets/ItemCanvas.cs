using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCanvas : MonoBehaviour
{

    public Weapon weapon;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI line1;
    public TextMeshProUGUI line2;
    public TextMeshProUGUI line3;
    public TextMeshProUGUI value1;
    public TextMeshProUGUI value2;
    public TextMeshProUGUI value3;
    public Image itemSprite;

    public void Start()
    {
        UpdateCanvas();
    }

    public void UpdateCanvas()
    {
        itemName.text = weapon.w_name;
        itemType.text = weapon.type.ToString();
        line1.text = "Damage";
        value1.text = weapon.damage.ToString();
        line2.text = "Accuracy";
        value2.text = (weapon.accuracy * 100).ToString() + "%";
        itemSprite.sprite = weapon.attackIconSprite;
    }
}
