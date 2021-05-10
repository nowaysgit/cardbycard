using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoot : Inventory
{
    [HideInInspector]
    public InventoryUI UI;

    protected override void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UI = game.inventoryUILoot;
    }
}
