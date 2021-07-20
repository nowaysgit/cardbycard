using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Game = GameManager;

public class CardShop : CardBase
{
    [Header("Inventory Class")]
    public InventoryLoot inventory;
    public override float Event(float getdamage)
    {
        if(IsBlocked) { return 0.0f; }
        Game.UIManager.UILoot.gameObject.SetActive(true);
        GenerateItems();
        return 0.0f;
        //sprite.sprite = Resources.Load<Sprite>(sprite.sprite.texture.name+"Open");
    }
    public override void Die()
    {
        base.Die();
        for (int i = 0; i < 3; i++)
        {
            inventory.Remove(i);
        }
        Destroy(gameObject);
    }
    public void GenerateItems()
    {
        Game.UIManager.UILoot.UpdateInventory("Clear", UiName: Info.title);
        var count = 3;
        var slot = 0;
        for (int i = 0; i < count; i++)
        {
            Item item = Game.FactoryItem.Make(-1, "Shop");
            item.gameObject.transform.SetParent(gameObject.transform);
            item.Slot = slot;
            inventory.Add(item, slot);
            Game.UIManager.UILoot.UpdateInventory("Add", slot, item);
            slot++;
        }
    }
}
