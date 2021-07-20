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
    [SerializeField] private Image[] TimeOut;
    [SerializeField] private GameObject[] SlotsFrame;
    [SerializeField] private Text[] SlotsTime;
    public virtual void Awake()
    {
        Game.singletone.OnAbilityClick.AddListener(AbilityClick);
        Game.singletone.OnInventoryRemove.AddListener(Remove);
        Game.singletone.OnGameEnd.AddListener(Reload);
        Game.singletone.OnAbilityTime.AddListener(AbilityTime);
    }


    private void Reload()
    {
        for (int i = 0; i < 3; i++)
        {
            Slots[i].sprite = Resources.Load<Sprite>("TheNULL");
            Slots[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
    private void Remove(int slot)
    {
        Slots[slot].sprite = Resources.Load<Sprite>("TheNULL");
        if (slot == 6)
        {
            Slots[slot].gameObject.transform.GetChild(1).GetComponent<Text>().text = "";
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
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = (Game.singletone.Player.InventoryEquipment.Count[item.Info.id]).ToString();

                    if (item.Info.type == "Weapon")
                    {
                        Slots[slot].gameObject.transform.GetChild(1).GetComponent<Text>().text = (item.Info.damage).ToString();
                    }
                }
                break;
            case "Update":
                if (item.Count > 1)
                {
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Count.ToString();
                }
                else
                {
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                break;
            default:
                break;
        }
    }
    public void UpdateKd(int slot, int kd, int maxKd)
    {
        TimeOut[slot - 3].gameObject.transform.GetChild(0).GetComponent<Text>().text = kd.ToString();
        TimeOut[slot - 3].fillAmount = Convert.ToSingle(kd) / Convert.ToSingle(maxKd);
        if (kd == 0)
        {
            TimeOut[slot - 3].gameObject.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
    public virtual void OnClick(int slot = 0)
    {
        Game.singletone.Player.Inventory.Use(slot, Game.singletone.Player);
    }
    private void AbilityClick(int slot = 0)
    {
        SlotsFrame[slot].SetActive(!(SlotsFrame[slot].activeSelf));
    }
    private void AbilityTime(int slot = 0, int count = 0)
    {
        if(count <= 0)
        {
            SlotsTime[slot].text = "";
        }
        else
        {
            SlotsTime[slot].text = count.ToString();
        }
    }
}

