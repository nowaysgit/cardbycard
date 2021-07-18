using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UIShop : MonoBehaviour
{
    [Header("Info Panel")]
    [SerializeField] private Text ItemNameText;
    [SerializeField] private Text ItemDescriptionText;
    [SerializeField] private Text ItemPriceText;

    [Header("Buttons")]
    [SerializeField] private Button buttonWeapon;
    [SerializeField] private Button buttonAbilities;

    [Header("Item Prefab")]
    [SerializeField] private GameObject itemUIPrefab;

    [Header("Tabs")]
    [SerializeField] private GameObject listWeapons;
    [SerializeField] private GameObject listAbilities;

    protected InventoryEquipment inventoryEquipment;
    private bool isInitialized = false;
    private int chooseItemId;
    public bool isActive {get; private set;}

    private Dictionary<int, GameObject> buttons = new Dictionary<int, GameObject>();

    private void Awake() 
    {
        inventoryEquipment = Game.singletone.Player.GetComponent<InventoryEquipment>();
        Game.singletone.OnGameEnd.AddListener(Load);
        gameObject.SetActive(false);
    }
    public void Load()
    {
        foreach (InfoItem item in Game.Data.ItemList) //filling with all kinds of items 
        {
            if (item.type == "Weapon" || item.type == "Ability")
            {
                if (inventoryEquipment.Count.ContainsKey(item.id) && !(buttons.ContainsKey(item.id)))
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
        }
        ReloadAll();
    }
    public void ReloadAll()
    {
        inventoryEquipment = Game.singletone.Player.GetComponent<InventoryEquipment>();
        foreach (var but in buttons)
        {
            var count = inventoryEquipment.Count[but.Key];
            but.Value.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        }
    }
    public void Reload(int key)
    {
        var but = buttons[key];
        var count = inventoryEquipment.Count[key];
        but.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
    }
    private GameObject MakeButton(InfoItem item)
    {
        GameObject button = (GameObject)Instantiate(itemUIPrefab) as GameObject;
        button.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spriteName);
        button.transform.GetChild(2).GetComponent<Text>().text = item.title;
        var count = inventoryEquipment.Count[item.id];
        button.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        button.GetComponent<Button>().onClick.AddListener(() => OnClick(item.id));
        return button;
    }
    public void OnClick(int id)
    {
        chooseItemId = id;
        ItemNameText.text = Game.Data.ItemList[id].title;
        ItemDescriptionText.text = Game.Data.ItemList[id].description;
        ItemPriceText.text = Game.Data.ItemList[id].cost.ToString();
    }
    public void OnCLickTabs(bool isTrue) //tab 0 == true
    {
        listWeapons.SetActive(isTrue);
        buttonWeapon.interactable = !isTrue;
        listAbilities.SetActive(!isTrue);
        buttonAbilities.interactable = isTrue;
    }
    public void TryBuy()
    {
        if (chooseItemId == 0) return;
        var item = Game.Data.ItemList[chooseItemId];
        var cost = item.cost;
        if (cost > 0)
        {
            if (Game.singletone.Player.Money >= cost)
            {
                Game.singletone.Player.Money -= cost;
                Game.singletone.Player.InventoryEquipment.Upgrade(item.id);
                Game.singletone.OnEquipmentAdd.Invoke(item.id);
            }
        }
        else
        {
            Game.singletone.Player.InventoryEquipment.Upgrade(item.id);
            Game.singletone.OnEquipmentAdd.Invoke(item.id);
        }
        Reload(item.id);
    }
}
