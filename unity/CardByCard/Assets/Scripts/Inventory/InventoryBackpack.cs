using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class InventoryBackpack : MonoBehaviour
{
    public Dictionary<int, int> Count = new Dictionary<int, int>();
    
    private void Awake()
    {
        foreach(InfoItem item in Game.Data.ItemList)
        {
            if (item.type == "Weapon" || item.type == "Ability")
            {
                Count.Add(item.id, 0);
            }
        }
    }
    public void Add(int id, int type = 1)
    {
        Count[id]++;
        Game.UIManager.UIBackpack.Reload(id);
        for (int i = 3; i < 7; i++)
        {
            if(Game.ControllerField.Player.Inventory.Items.ContainsKey(i) && Game.ControllerField.Player.Inventory.Items[i].Info.id == id) 
            {
                Game.UIManager.UIBackpack.OnClick(id);
            }            
        }
    }
}
