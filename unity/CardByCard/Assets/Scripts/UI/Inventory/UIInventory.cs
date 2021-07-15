using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game = GameManager;

public class UIInventory : MonoBehaviour
{
    protected Image[] Slots;
    public virtual void Awake()
    {
        var countChild = transform.childCount;
        Slots = new Image[countChild];
        for (int i = 0; i < countChild; i++)
        {
            Slots[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }
    public virtual void UpdateInventory(string act, int slot = 0, Item item = null, bool isShop = false, string UiName = "Corpse")
    {
        switch (act)
        {
            case "Add":
                Slots[slot].sprite = Resources.Load<Sprite>(item.Info.spriteName);
                if (item.Info.type == "Weapon" || item.Info.type == "Ability")
                {
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = (Game.ControllerField.Player.InventoryBackpack.Count[item.Info.id]).ToString();

                    if(item.Info.type == "Weapon")
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
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Clear":
                foreach (var img in Slots)
                {
                    img.sprite = Resources.Load<Sprite>("TheNULL");
                    img.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    public virtual void OnClick(int slot = 0)
    {
        Game.ControllerField.Player.Inventory.Use(slot, Game.ControllerField.Player.gameObject);
    }
}

