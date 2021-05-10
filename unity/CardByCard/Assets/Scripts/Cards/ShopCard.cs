using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopCard : Card
{
    [Header("Inventory Class")]
    public InventoryLoot inventory;
    public override void Event(float getdamage, out float givedamage, out bool canmove)
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = true; }
        game.inventoryUILoot.gameObject.SetActive(true);
        GenerateItems();
        canmove = true;
        givedamage = 0.0f;
        //sprite.sprite = Resources.Load<Sprite>(sprite.sprite.texture.name+"Open");
    }
    public override void Die()
    {
        IsBlocked = true;
        for (int i = 0; i < 3; i++)
        {
            inventory.Remove(i);
        }
        Instantiate(FxDie, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void GenerateItems()
    {
        game.inventoryUILoot.UpdateInventory("Clear", UiName: Info.title);
        var count = 3;
        var slot = 0;
        for (int i = 0; i < count; i++)
        {
            Item item = game.MakeItem(-1, "Shop");
            item.gameObject.transform.SetParent(gameObject.transform);
            item.Slot = slot;
            inventory.Add(item, slot);
            game.inventoryUILoot.UpdateInventory("Add", slot, item, true);
            slot++;
        }
    }
}
