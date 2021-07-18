using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class InventoryEquipment : MonoBehaviour
{
    public Dictionary<int, int> Count = new Dictionary<int, int>();
    private ControllerPlayer Player;

    private void Awake()
    {
        //LOAD INVENTORY
    }
    public void Add(InfoItem item)
    {
        Count.Add(item.id, 0);
        Game.singletone.OnEquipmentAdd.Invoke(item.id);
    }
    public void Upgrade(int id)
    {
        Count[id]++;
        Game.singletone.OnEquipmentAdd.Invoke(id);
    }
}
