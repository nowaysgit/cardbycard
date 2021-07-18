using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game = GameManager;

public class FactoryItem : FactoryBase
{
    [Header("Chance LVL Settings")]
    [SerializeField] public int[] LvlProgression;

    [Header("Valide Items")]
    [SerializeField] public List<InfoItem> ValideItems;
    
    [Header("Blocked Items Lvl")]
    public int[] BlockedItemsLvl = new int[4];

    [Header("Prefabs")]
    [SerializeField] private GameObject itemPrefab;

    private void Awake()
    {
        foreach (var item in Game.Data.ItemList)
        {
            if (!(item.type == "Ability" || item.type == "Weapon")) continue;
            if(Game.singletone.Player.InventoryEquipment.Count.ContainsKey(item.id)) continue;
            ValideItems.Add(item);
        }
        Game.singletone.OnEquipmentAdd.AddListener(OnEquipmentAdd);
    }
    public void OnEquipmentAdd(int id)
    {
        ValideItems.Remove(Game.Data.ItemList[id]);
    }

    public Item Make(int _itemID = -1, string _rulesType = "Corpse")
    {
        InfoItem infoItem;
        if (_itemID == -1) infoItem = MakeInfo(_rulesType);
        else infoItem = Game.Data.ItemList[_itemID];

        return spawn(infoItem);
    }
    public InfoItem MakeInfo(string rulesType)
    {
        InfoItem item = Game.Data.ItemList[0];
        var rand = Random.Range(0, 100);
        switch (rulesType)
        {
            case "Corpse":
                {
                    if (rand < 10 && (BlockedItemsLvl.Sum() != BlockedItemsLvl.Length))// Weapons and abilities
                    {
                        bool find = false;
                        int iteration = 0;
                        while (!find || iteration != 100)
                        {
                            iteration++;
                            if((BlockedItemsLvl.Sum() == BlockedItemsLvl.Length))
                            {
                                find = true;
                                break;
                            }
                            var itemLvl = RandomRules(LvlProgression);

                            if(BlockedItemsLvl[itemLvl] != 0) //if items itemLvl is gone
                            {
                                continue;
                            }

                            InfoItem[] itemsLvl = ValideItems.Select(s => s).Where(s => s.lvl == itemLvl).ToArray();
                            if (itemsLvl.Length > 0) 
                            {
                                item = itemsLvl[Random.Range(0, itemsLvl.Length)]; 
                                find = true;
                                break;
                            }
                            else
                            {
                                BlockedItemsLvl[itemLvl]++;
                            }
                        }
                    }
                    else if (rand < 25) // HALF HEAL 85
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[2];
                        }
                        else item = Game.Data.ItemList[4];
                    }
                    else if (rand < 30) // FULL HEAL 90
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = Game.Data.ItemList[1];
                        }
                        else item = Game.Data.ItemList[3];
                    }
                    else
                    {
                        item = Game.Data.ItemList[0];
                        item.cost = Random.Range(1, 16);
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
