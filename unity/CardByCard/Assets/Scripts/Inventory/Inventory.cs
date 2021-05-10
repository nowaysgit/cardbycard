using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
	
    [Header("Settings")]
    [Range(1, 9)]
    public int size;
    public Dictionary<int, Item> Items = new Dictionary<int, Item>();
    protected GameController game;
    protected int lastSlot;

    protected virtual void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        lastSlot = Items.Count;
    }
    protected bool isEmpty(int slot)
    {
        if (slot == -1) slot = lastSlot;
        if (Items.ContainsKey(slot)) return false;
        return true;
    }
    public virtual bool Add(Item item, int slot = -1)
    {
        if (slot == -1) slot = lastSlot;

        item.Slot = slot;
        Items.Add(slot, item);
        return true;
    }
    public virtual void Remove(int slot = -1)
    {
        if (isEmpty(slot)==true) return;
        Items.Remove(slot);
    }
    public virtual void Use(int slot, GameObject receiver)
    {
        if (isEmpty(slot)) return;
        if (!Items[slot]) return;
        Items[slot].Event(gameObject, receiver);
    }
}


