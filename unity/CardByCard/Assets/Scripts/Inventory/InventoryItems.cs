using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItems : MonoBehaviour
{
    public Dictionary<int, int> Count = new Dictionary<int, int>();
    protected GameController game;
    
    private void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        foreach(ItemInfo item in game.data.ItemList)
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
        game.inventoryUIItems.Reload(id);
        for (int i = 3; i < 7; i++)
        {
            if(game.player.inventory.Items.ContainsKey(i) && game.player.inventory.Items[i].Info.id == id) 
            {
            game.inventoryUIItems.OnClick(id);
            }            
        }
    }
}
