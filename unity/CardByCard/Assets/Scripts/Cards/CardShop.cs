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
    public override void Event(float getdamage, out float givedamage, out bool canmove)
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = true; }
        Game.UIManager.UILoot.gameObject.SetActive(true);
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
        OnDied.Invoke();
        Instantiate(FxDie, transform.position, Quaternion.identity);
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
            Game.UIManager.UILoot.UpdateInventory("Add", slot, item, true);
            slot++;
        }
    }
}
