using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game = GameManager;

public class UIInventory : MonoBehaviour
{
    [SerializeField] protected Image[] Slots;
    [SerializeField] protected Image[] TimeOut;
    public virtual void Awake()
    {
        var countChild = transform.childCount;
        Slots = new Image[countChild];
        for (int i = 0; i < countChild; i++)
        {
            Slots[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }
    public virtual void UpdateInventory(string act, int slot = 0, Item item = null, string UiName = "Corpse")
    {
        switch (act)
        {
            case "Add":
                Slots[slot].sprite = Resources.Load<Sprite>(item.Info.spriteName);
                if (item.Info.type == "Weapon" || item.Info.type == "Ability")
                {
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = (Game.singletone.Player.InventoryEquipment.Count[item.Info.id]).ToString();

                    if (item.Info.type == "Weapon")
                    {
                        Slots[slot].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        Slots[slot].gameObject.transform.GetChild(1).GetComponent<Text>().text = (item.Info.damage).ToString();
                    }
                }
                break;
            case "Remove":
                Slots[slot].sprite = Resources.Load<Sprite>("TheNULL");
                if (slot == 6)
                {
                    Slots[slot].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                }
                break;
            case "Update":
                if (item.Count > 1)
                {
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Count.ToString();
                }
                else
                {
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;
            case "Clear":
                for (int i = 0; i < 3; i++)
                {
                    Slots[i].sprite = Resources.Load<Sprite>("TheNULL");
                    Slots[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    public void UpdateKd(int slot, int kd, int maxKd)
    {
        TimeOut[slot-3].gameObject.transform.GetChild(0).GetComponent<Text>().text = kd.ToString();
        TimeOut[slot-3].fillAmount = Convert.ToSingle(kd)/Convert.ToSingle(maxKd);
        if (kd == 0) 
        {
            TimeOut[slot-3].gameObject.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
    public virtual void OnClick(int slot = 0)
    {
        Game.singletone.Player.Inventory.Use(slot, Game.singletone.Player.gameObject);
    }
}

