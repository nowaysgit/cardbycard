using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Info")]
    public ItemInfo Info;
    public int Count;
    public int Slot;

    [HideInInspector]
    protected GameController game;
    protected Ability ability;
    protected Weapon weapon;

    private void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ability = GameObject.FindGameObjectWithTag("GameController").GetComponent<Ability>();
        weapon = GameObject.FindGameObjectWithTag("GameController").GetComponent<Weapon>();
        Count = 1;
    }
    public void Load(ItemInfo _info)
    {
        Info = game.data.CopyFromSerialize<ItemInfo>(_info);
    }
    public void Event(GameObject attacking, GameObject receiver)
    {
        switch (Info.type)
        {
            case "Disposable":
                if (Count >= 1) Count--; game.inventoryUI.UpdateInventory("Update", Slot, this);
                break;
            case "Weapon":
                weapon.UseWeapon(Info.typeAbility, this, attacking, receiver);
                break;
            default:
                break;
        }
        if (Info.typeAbility != "none")
        {
            ability.UseAbility(Info.typeAbility, this, attacking, receiver);
        }
        if (Count < 1) { game.inventoryUI.UpdateInventory("Remove", Slot, this); Destroy(gameObject); }
    }

}
