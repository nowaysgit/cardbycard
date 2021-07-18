using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(ControllerPlayer))]

public class InventoryPlayer : InventoryBase
{
    protected ControllerPlayer player;
    protected override void Awake()
    {
        player = gameObject.GetComponent<ControllerPlayer>();
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
                else if (Items.ContainsKey(i) && Items[i] == null && !emptyFound) { Items.Remove(i); Game.UIManager.UIInventory.UpdateInventory("Remove", i); slot = i; emptyFound = true; }
                else if (Items.ContainsKey(i) && item.Info.id == Items[i].Info.id)
                {
                    if (Items[i].Count < item.Info.maxInStack)
                    {
                        Items[i].Count++;
                        Game.UIManager.UIInventory.UpdateInventory("Update", i, Items[i]);
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
        if (item.Info.type == "Weapon")
        {
            item.Info.damage = Mathf.Round(item.Info.damage += (item.Info.damage * player.InventoryEquipment.Count[item.Info.id]) * 0.3f);
            player.Damage = item.Info.damage;
        }
        else if (item.Info.type == "Ability")
        {
            item.Info.damage = Convert.ToSingle(Math.Round(item.Info.damage += (item.Info.damage * player.InventoryEquipment.Count[item.Info.id]) * 0.3f, 2));
            item.Info.manaCost = Mathf.Round(item.Info.manaCost += (item.Info.manaCost * player.InventoryEquipment.Count[item.Info.id]) * 0.3f);
            //player.Shield = item.Info.damage;
            //player.ManaCost = item.Info.damage;
        }
        item.InInventory = true;
        Items.Add(slot, item);
        Game.UIManager.UIInventory.UpdateInventory("Add", slot, item);
        return true;
    }
    public override void Remove(int slot = -1)
    {
        if (isEmpty(slot)) return;
        if (Items[slot] != null)
        {
            Destroy(Items[slot].gameObject);
            Items[slot].InInventory = false;
        }
        Items.Remove(slot);
        Game.UIManager.UIInventory.UpdateInventory("Remove", slot);
    }
    public void ChangeSlot(int slot, int toSlot)
    {
        if (isEmpty(slot)) return;

        var temp = Items[slot];

        Game.UIManager.UIInventory.UpdateInventory("Remove", slot);
        if (Items.ContainsKey(toSlot) && Items[toSlot] != null)
        {
            Game.UIManager.UIInventory.UpdateInventory("Add", slot, Items[toSlot]);
        }
        
        if (Items.ContainsKey(toSlot) && Items[toSlot] != null)
        {
            Items[slot] = Items[toSlot];
        }
        else
        {
            Items[slot] = null;
        }
        Items[toSlot] = temp;

        Game.UIManager.UIInventory.UpdateInventory("Remove", toSlot);
        Game.UIManager.UIInventory.UpdateInventory("Add", toSlot, temp);
    }
    public override void Use(int _slot, GameObject _attacker)
    {
        if (isEmpty(_slot)) return;
        if (!Items[_slot]) return;
        if (Items[_slot].Info.type == "Ability" && Items[_slot].Info.attackMethod == "choice")
        {
            if (Items[_slot].Kd != 0)
            {
                Debug.Log("KD: " + Items[_slot].Kd + "/" + Items[_slot].Info.kd);
                return;
            }
            Items[_slot].IsClick = !Items[_slot].IsClick;
            Debug.Log("isClick Status: " + Items[_slot].IsClick);
            if (Items[_slot].IsClick)
            {
                Game.singletone.OnCardClick.AddListener(() => { AbilityStart(_slot); });
            }
            else
            {
                Game.singletone.OnCardClick.RemoveAllListeners();
            }
        }
        else
        {
            Items[_slot].Event(gameObject, _attacker);
        }
    }
    private void AbilityStart(int _slot)
    {
        Debug.Log("AbilityStarted");
        if (isEmpty(_slot)) return;
        if (!Items[_slot]) return;
        Items[_slot].Event(gameObject, Game.singletone.GameStateInGame.LastCardClick);
        Items[_slot].Kd = Items[_slot].Info.kd;
        Game.UIManager.UIInventory.UpdateKd(_slot, Items[_slot].Info.kd, Items[_slot].Info.kd);
        Items[_slot].IsClick = false;
        Game.singletone.OnCardClick.RemoveAllListeners();
        Debug.Log("isClick Status: " + Items[_slot].IsClick);
        Debug.Log("KD: " + Items[_slot].Kd + "/" + Items[_slot].Info.kd);
    }
}
