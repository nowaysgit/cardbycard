using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UIBackpack : MonoBehaviour
{
    [Header("Active Slots")]
    [SerializeField] private Image[] slots;

    [Header("Buttons")]
    [SerializeField] private Button buttonWeapon;
    [SerializeField] private Button buttonAbilities;

    [Header("Item Prefab")]
    [SerializeField] private GameObject itemUIPrefab;

    [Header("Tabs")]
    [SerializeField] private GameObject listWeapons;
    [SerializeField] private GameObject listAbilities;

    protected InventoryBackpack inventory;
    private int slotAbility = 0;
    private bool isActive = false;
    private bool isInitialized = false;

    private Dictionary<int, GameObject> buttons = new Dictionary<int, GameObject>();

    public void Load()
    {
        inventory = Game.ControllerField.Player.GetComponent<InventoryBackpack>();
        if (isInitialized) return;
        foreach (InfoItem item in Game.Data.ItemList) //filling with all kinds of items 
        {
            if (item.type == "Weapon" || item.type == "Ability")
            {
                GameObject button = MakeButton(item);
                if (item.type == "Weapon")
                {
                    button.transform.SetParent(listWeapons.transform.GetChild(0).GetChild(0), false);
                }
                else
                {
                    button.transform.SetParent(listAbilities.transform.GetChild(0).GetChild(0), false);
                }
                buttons.Add(item.id, button);
            }
        }
        isInitialized = true;
    }
    public void ReloadAll()
    {
        for (int i = 0; i < 4; i++)
        {
            slots[i].sprite = Resources.Load<Sprite>("TheNULL");
        }
        inventory = Game.ControllerField.Player.GetComponent<InventoryBackpack>();
        foreach (var but in buttons)
        {
            var count = inventory.Count[but.Key];
            if (count > 0) { but.Value.GetComponent<Button>().interactable = true; }
            else { but.Value.GetComponent<Button>().interactable = false; }
            but.Value.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        }
    }
    public void Reload(int key)
    {
        var but = buttons[key];
        var count = inventory.Count[key];
        if (count > 0) { but.GetComponent<Button>().interactable = true; }
        but.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
    }
    private GameObject MakeButton(InfoItem item)
    {
        GameObject button = (GameObject)Instantiate(itemUIPrefab) as GameObject;
        button.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spriteName);
        button.transform.GetChild(2).GetComponent<Text>().text = item.title;
        var count = inventory.Count[item.id];
        if (count == 0) { button.GetComponent<Button>().interactable = false; }
        button.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        button.GetComponent<Button>().onClick.AddListener(() => OnClick(item.id));
        return button;
    }
    public void OnClick(int id)
    {
        var item = Game.FactoryItem.Make(id);
        if (item.Info.type == "Weapon")
        {
            Game.ControllerField.Player.Inventory.Add(item, 6);
            slots[3].sprite = Resources.Load<Sprite>(item.Info.spriteName);
        }
        else
        {
            Game.ControllerField.Player.Inventory.Add(item, slotAbility + 3);
            slots[slotAbility].sprite = Resources.Load<Sprite>(item.Info.spriteName);
        }
    }
    public void OnCLickTabs(int tab = 0)
    {
        if (tab == 0)
        {
            listWeapons.SetActive(true);
            listAbilities.SetActive(false);
            buttonWeapon.interactable = false;
            buttonAbilities.interactable = true;
        }
        else
        {
            listWeapons.SetActive(false);
            listAbilities.SetActive(true);
            buttonWeapon.interactable = true;
            buttonAbilities.interactable = false;
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
    public void Open()
    {
        if (isActive) { gameObject.SetActive(false); isActive = false; }
        else { gameObject.SetActive(true); isActive = true; }
    }
}
