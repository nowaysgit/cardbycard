using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(ControllerPlayer))]

public class InventoryPlayer : InventoryBase
{
    protected ControllerPlayer player;
    private int ActiveChooseSlot;
    protected override void Awake()
    {
        ActiveChooseSlot = -1;
        player = gameObject.GetComponent<ControllerPlayer>();
        lastSlot = Items.Count;
        Game.singletone.OnGamePreparation.AddListener(ReloadInventory);
    }
    private void ReloadInventory()
    {
        for (int i = 0; i < 7; i++)
        {
            if (!isEmpty(i))
            {
                Items[i].Kd = 0;
                if (Items[i].Info.type == "Disposable")
                {
                    Remove(i);
                }
            }
        }
    }
    public override bool Add(Item item, int slot = -1)
    {
        if (item.Info.type == "Disposable")
        {
            var emptyFound = false;
            for (int i = 0; i < 3; i++)
            {
                if (!Items.ContainsKey(i) && !emptyFound) { slot = i; emptyFound = true; }
                else if (Items.ContainsKey(i) && Items[i] == null && !emptyFound)
                {
                    Items.Remove(i);
                    Game.singletone.OnInventoryRemove.Invoke(i);
                    slot = i;
                    emptyFound = true;
                }
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
        if (isEmpty(slot)) 
        {
            return;
        }
        if (Items[slot] != null)
        {
            Items[slot].InInventory = false;
            Destroy(Items[slot].gameObject);
        }
        Items.Remove(slot);
        Game.singletone.OnInventoryRemove.Invoke(slot);
    }
    public void ChangeSlot(int slot, int toSlot)
    {
        if (isEmpty(slot)) return;

        var temp = Items[slot];

        Game.singletone.OnInventoryRemove.Invoke(slot);
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

        Game.singletone.OnInventoryRemove.Invoke(toSlot);
        Game.UIManager.UIInventory.UpdateInventory("Add", toSlot, temp);
    }
    public override void Use(int _slot, Interactive _attacker)
    {
        if (isEmpty(_slot)) return;
        if (!Items[_slot]) return;
        if (Items[_slot].Info.type == "Ability" && Items[_slot].Info.attackMethod == "choice")
        {
            if (Items[_slot].Kd != 0)
            {
                return;
            }

            Items[_slot].IsClick = !Items[_slot].IsClick;

            Game.singletone.OnAbilityClick.Invoke(_slot);

            if (Items[_slot].IsClick) //PRESSED SLOT ITEM IS ACTIVE
            {
                if (ActiveChooseSlot != -1 && Items[ActiveChooseSlot].IsClick)
                {
                    Items[ActiveChooseSlot].IsClick = !Items[ActiveChooseSlot].IsClick;
                    Game.singletone.OnAbilityClick.Invoke(ActiveChooseSlot);
                    Game.singletone.OnCardClick.RemoveAllListeners();
                }
                Game.singletone.OnCardClick.AddListener(() => { AbilityStart(_slot); });
                ActiveChooseSlot = _slot;
            }
            else //PRESSED SLOT ITEM IS INACTIVE
            {
                if (ActiveChooseSlot == _slot)
                {
                    ActiveChooseSlot = -1;
                }
                Game.singletone.OnCardClick.RemoveAllListeners();
            }
        }
        else if (Items[_slot].Info.type == "Ability" && Items[_slot].Info.attackMethod == "passive")
        {
            if (Items[_slot].Kd != 0)
            {
                return;
            }
            Items[_slot].IsClick = !Items[_slot].IsClick;
            Game.singletone.OnAbilityClick.Invoke(_slot);
            
            Items[_slot].Event(player, _attacker);
        }
        else
        {
            Items[_slot].Event(player, _attacker);
        }
    }
    private void AbilityStart(int _slot)
    {
        if (isEmpty(_slot)) return;
        if (!Items[_slot]) return;
        if (Game.singletone.GameStateInGame.LastCardClick.Info.type != "Enemy") return;

        Items[_slot].Event(Game.singletone.Player, Game.singletone.GameStateInGame.LastCardClick);

        Items[_slot].Kd = Items[_slot].Info.kd;
        Game.UIManager.UIInventory.UpdateKd(_slot, Items[_slot].Info.kd, Items[_slot].Info.kd);

        Items[_slot].IsClick = false;
        Game.singletone.OnAbilityClick.Invoke(_slot);
        Game.singletone.OnCardClick.RemoveAllListeners();
        ActiveChooseSlot = -1;
    }
}
