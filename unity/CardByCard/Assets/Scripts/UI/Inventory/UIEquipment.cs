using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UIEquipment : MonoBehaviour
{
    [Header("Active Slots")]
    [SerializeField] private Image[] slots;

    [Header("Active Slots BackGround")]
    [SerializeField] private Image[] slotsBackGround;

    [Header("Buttons")]
    [SerializeField] private Button buttonWeapon;
    [SerializeField] private Button buttonAbilities;

    [Header("Item Prefab")]
    [SerializeField] private GameObject itemUIPrefab;

    [Header("Tabs")]
    [SerializeField] private GameObject listWeapons;
    [SerializeField] private GameObject listAbilities;

    protected InventoryEquipment inventoryEquipment;
    private int slotAbility = 0;
    private bool isActive = false;
    private bool isInitialized = false;

    private Dictionary<int, GameObject> buttons = new Dictionary<int, GameObject>();
    private void Awake()
    {
        inventoryEquipment = Game.singletone.Player.GetComponent<InventoryEquipment>();
        Game.singletone.OnGameEnd.AddListener(Load);
        Game.singletone.OnEquipmentAdd.AddListener(Reload);
        //Load();
        gameObject.SetActive(false);
    }
    public void Load()
    {
        foreach (InfoItem item in Game.Data.ItemList) //filling with all kinds of items 
        {
            if (item.type == "Weapon" || item.type == "Ability")
            {
                if (inventoryEquipment.Count.ContainsKey(item.id) && (inventoryEquipment.Count[item.id] > 0) && !(buttons.ContainsKey(item.id)))
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
    }
    public void ReloadAll()
    {
        for (int i = 0; i < 4; i++)
        {
            slots[i].sprite = Resources.Load<Sprite>("TheNULL");
        }
        inventoryEquipment = Game.singletone.Player.GetComponent<InventoryEquipment>();
        foreach (var but in buttons)
        {
            var count = inventoryEquipment.Count[but.Key];
            if (count > 0) { but.Value.GetComponent<Button>().interactable = true; }
            else { but.Value.GetComponent<Button>().interactable = false; }
            but.Value.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
        }
    }
    public void Reload(int key)
    {
        if (!buttons.ContainsKey(key))
        {
            Add(key);
            return;
        }
        var but = buttons[key];
        var count = inventoryEquipment.Count[key];
        if (count > 0) { but.GetComponent<Button>().interactable = true; }
        but.transform.GetChild(3).GetComponent<Text>().text = count.ToString();
    }
    public void Add(int key)
    {
        var item = Game.Data.ItemList[key];
        if (item.type == "Weapon" || item.type == "Ability")
        {
            if (inventoryEquipment.Count.ContainsKey(item.id) && (inventoryEquipment.Count[item.id] > 0) && !(buttons.ContainsKey(item.id)))
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

    private void ReloadActiveSlot(int slot)
    {
        if ((Game.singletone.Player.Inventory.Items.ContainsKey(slot + 3) && Game.singletone.Player.Inventory.Items[slot + 3] != null))
        {
            slots[slot].sprite = Resources.Load<Sprite>(Game.singletone.Player.Inventory.Items[slot + 3].Info.spriteName);
        }
        else
        {
            slots[slot].sprite = Resources.Load<Sprite>("TheNULL");
        }
    }
    private GameObject MakeButton(InfoItem item)
    {
        GameObject button = (GameObject)Instantiate(itemUIPrefab) as GameObject;
        button.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spriteName);
        button.transform.GetChild(2).GetComponent<Text>().text = item.title;
        var count = inventoryEquipment.Count[item.id];
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
            Game.singletone.Player.Inventory.Add(item, 6);
            slots[3].sprite = Resources.Load<Sprite>(item.Info.spriteName);
            return;
        }
        else
        {
            var canCreate = true;
            for (int i = 2; i < 6; i++)
            {
                if (Game.singletone.Player.Inventory.Items.ContainsKey(i) && Game.singletone.Player.Inventory.Items[i] != null && Game.singletone.Player.Inventory.Items[i].Info.id == id)
                {
                    canCreate = false;
                    Game.singletone.Player.Inventory.ChangeSlot(i, slotAbility + 3);
                    Debug.Log("i " + i);
                    Debug.Log("slotAbility " + slotAbility);
                    ReloadActiveSlot(i - 3);
                    ReloadActiveSlot(slotAbility);
                    break;
                }
            }
            if (canCreate)
            {
                Game.singletone.Player.Inventory.Add(item, slotAbility + 3);
                slots[slotAbility].sprite = Resources.Load<Sprite>(item.Info.spriteName);
                return;
            }
        }
        Destroy(item.gameObject);
    }
    public void SetActive(int slot)
    {
        slotsBackGround[slotAbility].color = new Color32(147, 107, 0, 255);
        slotAbility = slot;
        slotsBackGround[slotAbility].color = new Color32(147, 87, 0, 255);
    }
    public void OnActive(int slot)
    {
        if (slot != 3) OnCLickTabs(false);
        else OnCLickTabs(true);
        SetActive(slot);
    }
    public void OnCLickTabs(bool isTrue) //tab 0 == true
    {
        listWeapons.SetActive(isTrue);
        buttonWeapon.interactable = !isTrue;
        listAbilities.SetActive(!isTrue);
        buttonAbilities.interactable = isTrue;
        if (isTrue)
        {
            SetActive(3);
        }
        else
        {
            SetActive(0);
        }
    }
}
