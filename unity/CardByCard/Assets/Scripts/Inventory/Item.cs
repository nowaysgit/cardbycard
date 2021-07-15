using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class Item : MonoBehaviour
{
    [Header("Info")]
    public int Count;
    public int Slot;
    public bool IsClick;
    public int Kd;

    [HideInInspector]
    public InfoItem Info { get; private set; }
    protected Ability ability;
    protected Weapon weapon;

    private void Awake()
    {
        Game.ControllerField.OnMoved.AddListener(isMoved);
        ability = GameObject.FindGameObjectWithTag("GameController").GetComponent<Ability>();
        weapon = GameObject.FindGameObjectWithTag("GameController").GetComponent<Weapon>();
        Count = 1;
    }
    public void Load(InfoItem _info)
    {
        Info = Game.Data.CopyFromSerialize<InfoItem>(_info);
    }
    public void Event(GameObject attacking, GameObject receiver)
    {
        switch (Info.type)
        {
            case "Disposable":
                if (Count >= 1) Count--; Game.UIManager.UIInventory.UpdateInventory("Update", Slot, this);
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
        if (Count < 1) { Game.UIManager.UIInventory.UpdateInventory("Remove", Slot, this); Destroy(gameObject); }
    }
    //5/5 0/5
    private void isMoved() //overridden by heirs and called after player change position
    {
        if(Kd != 0)
        {
            Kd--;
        }
    }

}
