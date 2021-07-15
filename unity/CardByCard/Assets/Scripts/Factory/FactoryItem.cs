using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class FactoryItem : FactoryBase
{
    [Header("Chance LVL Settings")]
    [SerializeField] public int[] LvlProgression;

    [Header("Prefabs")]
    [SerializeField] private GameObject itemPrefab;

    public Item Make(int _itemID = -1, string _rulesType = "Corpse")
    {
        InfoItem infoItem;
        if (_itemID == -1) infoItem = MakeInfo(_rulesType);
        else infoItem = Game.Data.ItemList[_itemID];

        return spawn(infoItem);
    }
    public InfoItem MakeInfo(string _rulesType)
    {
        InfoItem item;
        var rand = Random.Range(0, 100);
        switch (_rulesType)
        {
            case "Corpse":
                {
                    if (rand < 60) // MONEY
                    {
                        item = Game.Data.ItemList[0];
                        item.cost = Random.Range(1, 16);
                    }
                    else if (rand < 95) // HALF HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[2];
                        }
                        else item = Game.Data.ItemList[4];
                    }
                    else // FULL HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[1];
                        }
                        else item = Game.Data.ItemList[3];
                    }
                    return item;
                }
            case "Shop":
                {
                    if (rand < 20) // HALF HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[2];
                        }
                        else item = Game.Data.ItemList[4];
                    }
                    else if (rand < 30) // FULL HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[1];
                        }
                        else item = Game.Data.ItemList[3];
                    }
                    else// Weapons
                    {
                        var itemLvl = RandomRules(LvlProgression);
                        item = Game.Data.ItemList[Random.Range(5, 10)];
                        bool find = false;
                        while (!find)
                        {
                            if (item.lvl == itemLvl) 
                            {
                                find = true; 
                            }
                            item = Game.Data.ItemList[Random.Range(5, 10)];
                        }
                    }
                    return item;
                }
            default:
                {
                    item = Game.Data.ItemList[0];
                    return item;
                }
        }
    }
    private Item spawn(InfoItem _infoItem)
    {
        var itemGameObject = GameObject.Instantiate(Game.FactoryItem.itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var item = itemGameObject.GetComponent<Item>();
        item.Load(_infoItem);
        return item;
    }
}
