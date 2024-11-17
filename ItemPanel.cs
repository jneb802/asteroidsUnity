using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemDamage;

    public void BuildPanel(ItemData itemData)
    {
        itemName.text = itemData.itemName;
        itemDamage.text = itemData.damageValue.ToString();
    }
}
