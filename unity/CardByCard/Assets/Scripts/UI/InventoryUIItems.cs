using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItems : MonoBehaviour
{
	
    [Header("Active Slots")]
    public Image[] Slots;

    [Header("Buttons")]
    public Button ButtonWeapon;
    public Button ButtonAbilities;

    [Header("Item Prefab")]
    public GameObject itemUIPrefab;

    [Header("Tabs")]
    public GameObject listWeapons;
    public GameObject listAbilities;

    protected GameController game;
    protected InventoryItems inventoryItems;
    private int slotAbility = 0;
    private bool isActive = false;

    public Dictionary<int, GameObject> buttons = new Dictionary<int, GameObject>();

    public void Load()
    {
        if (game != null) return;
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        inventoryItems = game.player.GetComponent<InventoryItems>();
        foreach (ItemInfo item in game.data.ItemList) //filling with all kinds of items 
        {
            if (item.type == "Weapon")
            {
                GameObject button = MakeButton(item);
                button.transform.SetParent(listWeapons.transform.GetChild(0).GetChild(0), false);
                button.GetComponent<Button>().onClick.AddListener(() => OnClick(item.id));
                buttons.Add(item.id, button);
            }
            else if (item.type == "Ability")
            {
                GameObject button = MakeButton(item);
                button.transform.SetParent(listAbilities.transform.GetChild(0).GetChild(0), false);
                button.GetComponent<Button>().onClick.AddListener(() => OnClick(item.id));
                buttons.Add(item.id, button);
            }
        }
    }
    public void ReloadAll() 
    {
        for (int i = 0; i < 4; i++)
        {
            Slots[i].sprite = Resources.Load<Sprite>("TheNULL");
        }
        inventoryItems = game.player.GetComponent<InventoryItems>();
        foreach (var but in buttons)
        {
            var count = inventoryItems.Count[but.Key];
            if (count > 0) { but.Value.GetComponent<Button>().interactable = true; }
            else { but.Value.GetComponent<Button>().interactable = false; }
            but.Value.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        }
    }
    public void Reload(int key) 
    {
        var but = buttons[key];
        var count = inventoryItems.Count[key];
        if (count > 0) { but.GetComponent<Button>().interactable = true; }
        but.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
    }
    private GameObject MakeButton(ItemInfo item)
    {
        GameObject button = (GameObject)Instantiate(itemUIPrefab) as GameObject;
        button.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spriteName);
        button.transform.GetChild(2).GetComponent<Text>().text = item.title;
        var count = inventoryItems.Count[item.id];
        if (count == 0) { button.GetComponent<Button>().interactable = false; }
        button.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        return button;
    }
    public void OnClick(int id)
    {
        var item = game.MakeItem(id);
        if (item.Info.type == "Weapon")
        {
            game.player.inventory.Add(item, 6);
            Slots[3].sprite = Resources.Load<Sprite>(item.Info.spriteName);
        }
        else
        {
            game.player.inventory.Add(item, slotAbility+3);
            Slots[slotAbility].sprite = Resources.Load<Sprite>(item.Info.spriteName);
        }
    }
    private void OnLickTabs(int tab = 0)
    {
        if(tab == 0) {
            listWeapons.SetActive(true);
            listAbilities.SetActive(false);
            ButtonWeapon.interactable = false;
            ButtonAbilities.interactable = true;
        }
        else {
            listWeapons.SetActive(false);
            listAbilities.SetActive(true);
            ButtonWeapon.interactable = true;
            ButtonAbilities.interactable = false;
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
    public void Open()
    {
        if(isActive) { gameObject.SetActive(false); isActive = false; }
        else { gameObject.SetActive(true); isActive = true; }
    }
}
