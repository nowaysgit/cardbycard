using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Game = GameManager;

public class CardLoot : CardBase
{
    [Header("Inventory Class")]
    public InventoryLoot Inventory;
    public override float Event(float getdamage)
    {
        if (!Alive) { return 0.0f; }
        Game.UIManager.UILoot.gameObject.SetActive(true);
        GenerateItems();
        sprite.sprite = Resources.Load<Sprite>(sprite.sprite.texture.name + "Open");
        return 0.0f;
    }
    public override void Die()
    {
        if(!Alive) return;
        Game.UIManager.UILoot.gameObject.SetActive(false);
        base.Die();
        for (int i = 0; i < 3; i++)
        {
            Inventory.Remove(i);
        }
        Destroy(gameObject);
    }
    public void GenerateItems()
    {
        Game.UIManager.UILoot.UpdateInventory("Clear", UiName: Info.title);
        var count = Random.Range(1, 4);
        for (int slot = 0; slot < count; slot++)
        {
            Item item = Game.FactoryItem.Make();
            if (item.Info.type == "Weapon" || item.Info.type == "Ability")
            {
                bool find = false;
                if (slot == 0) find = true;
                while (!find)
                {
                    for (int i2 = 0; i2 < slot; i2++)
                    {
                        if (Inventory.Items[i2].Info.id == item.Info.id)
                        {
                            find = false;
                            item = Game.FactoryItem.Make();
                            if (!(item.Info.type == "Weapon" || item.Info.type == "Ability"))
                            {
                                find = true;
                            }
                            break;
                        }
                        else
                        {
                            find = true;
                        }
                    }
                }
            }
            item.gameObject.transform.SetParent(gameObject.transform);
            item.Slot = slot;
            Inventory.Add(item, slot);
            Game.UIManager.UILoot.UpdateInventory("Add", slot, item);
        }
    }
}
