using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : Inventory
{
    protected PlayerController  player;
    protected override void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player =  gameObject.GetComponent<PlayerController>();
        lastSlot = Items.Count;
    }
    public override bool Add(Item item, int slot = -1)
    {
        if (item.Info.type == "Disposable")
        {
            var emptyFound = false;
            for (int i = 0; i < 3; i++)
            {
                if (!Items.ContainsKey(i) && !emptyFound) { slot = i; emptyFound = true; }
                else if (Items.ContainsKey(i) && Items[i] == null && !emptyFound) { Items.Remove(i); game.inventoryUI.UpdateInventory("Remove", i); slot = i; emptyFound = true; }
                else if (Items.ContainsKey(i) && item.Info.id == Items[i].Info.id)
                {
                    if (Items[i].Count < item.Info.maxInStack)
                    {
                        Items[i].Count++;
                        game.inventoryUI.UpdateInventory("Update", i, Items[i]);
                        Destroy(item);
                        return true;
                    }
                    else continue;
                }
            }
        }
        else if (item.Info.type == "Money")
        {
            player.Money += Convert.ToInt32(item.Info.cost);
            Destroy(item);
            return true;
        }
        else
        {
            if (Items.ContainsKey(slot)) Remove(slot);
        }
        if (slot == -1) return false;
        item.Slot = slot;
        item.transform.SetParent(gameObject.transform);
        if(item.Info.type == "Weapon") 
        { 
            item.Info.damage = Mathf.Round(item.Info.damage+=(item.Info.damage*player.inventoryItems.Count[item.Info.id])*0.3f);
            player.Damage = item.Info.damage;
        }
        else if(item.Info.type == "Ability") 
        { 
            item.Info.damage = Convert.ToSingle(Math.Round(item.Info.damage+=(item.Info.damage*player.inventoryItems.Count[item.Info.id])*0.3f, 2));
            item.Info.manaCost = Mathf.Round(item.Info.manaCost+=(item.Info.manaCost*player.inventoryItems.Count[item.Info.id])*0.3f);
            player.Shield = item.Info.damage;
            player.ManaCost = item.Info.damage;
        }
        Items.Add(slot, item);
        game.inventoryUI.UpdateInventory("Add", slot, item);
        return true;
    }
    public override void Remove(int slot = -1)
    {
        if (isEmpty(slot)) return;
        Destroy(Items[slot].gameObject);
        Items.Remove(slot);
        game.inventoryUI.UpdateInventory("Remove", slot);
    }
}
