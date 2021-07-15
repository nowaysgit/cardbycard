using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
	
    [Header("Settings")]
    [Range(1, 9)]
    [SerializeField] private int size;
    public Dictionary<int, Item> Items = new Dictionary<int, Item>();
    protected int lastSlot;

    protected virtual void Awake()
    {
        lastSlot = Items.Count;
    }
    protected bool isEmpty(int _slot)
    {
        if (_slot == -1) _slot = lastSlot;
        if (Items.ContainsKey(_slot)) return false;
        return true;
    }
    public virtual bool Add(Item _item, int _slot = -1)
    {
        if (_slot == -1) _slot = lastSlot;

        _item.Slot = _slot;
        Items.Add(_slot, _item);
        return true;
    }
    public virtual void Remove(int _slot = -1)
    {
        if (isEmpty(_slot)==true) return;
        Items.Remove(_slot);
    }
    public virtual void Use(int _slot, GameObject _receiver)
    {
        if (isEmpty(_slot)) return;
        if (!Items[_slot]) return;
        Items[_slot].Event(gameObject, _receiver);
    }
}


