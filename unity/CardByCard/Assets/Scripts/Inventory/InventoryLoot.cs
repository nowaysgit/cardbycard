using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class InventoryLoot : InventoryBase
{
    [HideInInspector]
    public UIInventory UI;

    protected override void Awake()
    {
        UI = Game.UIManager.UILoot;
    }
}
