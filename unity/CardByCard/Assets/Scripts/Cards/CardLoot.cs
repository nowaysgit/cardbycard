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
    public override void Event(float getdamage, out float givedamage, out bool canmove)
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = true; }
        Game.UIManager.UILoot.gameObject.SetActive(true);
        GenerateItems();
        canmove = true;
        givedamage = 0.0f;
        sprite.sprite = Resources.Load<Sprite>(sprite.sprite.texture.name+"Open");
    }
    public override void Die()
    {
        IsBlocked = true;
        OnDied.Invoke();
        for (int i = 0; i < 3; i++)
        {
            Inventory.Remove(i);
        }
        Instantiate(FxDie, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void GenerateItems()
    {
        Game.UIManager.UILoot.UpdateInventory("Clear", UiName: Info.title);
        var count = Random.Range(1, 4);
        var slot = 0;
        for (int i = 0; i < count; i++)
        {
            Item item = Game.FactoryItem.Make();
            item.gameObject.transform.SetParent(gameObject.transform);
            item.Slot = slot;
            Inventory.Add(item, slot);
            Game.UIManager.UILoot.UpdateInventory("Add", slot, item);
            slot++;
        }
    }
}
