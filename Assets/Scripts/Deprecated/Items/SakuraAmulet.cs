using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuraAmulet : Item
{
    [SerializeField] private int HealtToHeal = 10;
    public  override void InventoryUse()
    {
        if(CanUse)Player.Instance.GetComponent<OldHealth>().Heal(HealtToHeal);
        CanUse = false;
    }

}
