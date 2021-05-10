using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUILoot : InventoryUI
{
    [Header("Items List")]
    public Item[] Items;

    [Header("Coin Settings")]
    public GameObject CoinIcon;
    public GameObject CoinIconPrefab;

    [Header("Color Settings")]
    public Color EmptyColor;
    public Color ItemColor;


    public int Count;

    private Button[] buttons;
    private float pxSize = 40;
    private Text uiName;
    private bool isShop;


    public override void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        var countChild = (transform.childCount) - 2;

        uiName = gameObject.transform.GetChild(countChild).gameObject.GetComponent<Text>();

        Items = new Item[countChild];
        Slots = new Image[countChild];
        buttons = transform.GetComponentsInChildren<Button>();
        buttons[buttons.Length-1] = null;
        for (int i = 0; i < countChild; i++)
        {
            var but = i;
            Slots[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            buttons[but].onClick.AddListener(() => OnClick(but));
        }
        Count = 0;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private bool playerAdd(Item item, int slot, int cost = 0)
    {
        if (cost > 0)
        {
            if (game.player.Money >= cost)
            {
                if (item.Info.type == "Disposable") return game.player.inventory.Add(item);
                else game.player.inventoryItems.Add(item.Info.id); return true;
            }
            else return false;
        }
        else
        {
            return game.player.inventory.Add(item);
        }
    }
    public override void OnClick(int slot = 0)
    {
        if (Items[slot] != null)
        {
            var cost = 0;
            if (isShop) cost = Items[slot].Info.cost;
            if (playerAdd(Items[slot], slot, cost))
            {
                game.player.Money -= cost;
                UpdateInventory("Remove", slot);
            }
        }
        if (isShop) { UpdateButtons(); }
        if (Count <= 0) gameObject.SetActive(false);
    }
    private void UpdateButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Items[i] != null)
            {
                if (Items[i].Info.cost > game.player.Money) { buttons[i].interactable = false; }
            }
        }
    }
    public override void UpdateInventory(string act, int slot = 0, Item item = null, bool IsShop = false, string UiName = "Corpse")
    {
        switch (act)
        {
            case "Add":
                {
                    Count++;
                    gameObject.SetActive(true);
                    buttons[slot].interactable = true;
                    Slots[slot].gameObject.SetActive(true);
                    Slots[slot].gameObject.transform.parent.gameObject.GetComponent<Image>().color = ItemColor;
                    isShop = IsShop;
                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Info.title.ToString();
                    if (isShop)
                    {
                        Slots[slot].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        Slots[slot].gameObject.transform.GetChild(2).gameObject.SetActive(true);
                        Slots[slot].gameObject.transform.GetChild(2).GetComponent<Text>().text = item.Info.cost.ToString();
                    }
                    else
                    {
                        Slots[slot].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        Slots[slot].gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    }
                    Slots[slot].sprite = Resources.Load<Sprite>(item.Info.spriteName);
                    Items[slot] = item;
                    Slots[slot].gameObject.transform.parent.gameObject.SetActive(true);
                    var rt = Slots[slot].gameObject.transform.parent.parent.gameObject.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x + 220, rt.sizeDelta.y);

                    if (item.Info.type == "Money")
                    {
                        var coin = Instantiate(CoinIconPrefab, Slots[slot].gameObject.transform.position, Quaternion.identity);

                        coin.transform.SetParent(CoinIcon.transform.parent, true);
                        coin.transform.localScale = CoinIcon.transform.localScale * 2;

                        coin.GetComponent<RectTransform>().pivot = CoinIcon.transform.GetComponent<RectTransform>().pivot;
                        coin.transform.GetChild(0).GetComponent<Text>().text = (item.Info.cost).ToString();

                        game.animatorUI.StartCoroutine(game.animatorUI.MoveUI(coin, CoinIcon, 15000f));

                        OnClick(slot);
                        Destroy(coin, 1.7f);
                    }
                    if (isShop) { UpdateButtons(); }
                }
                break;
            case "Remove":
                {
                    Slots[slot].sprite = Resources.Load<Sprite>("TheNULL");
                    Slots[slot].gameObject.transform.parent.gameObject.GetComponent<Image>().color = EmptyColor;
                    Items[slot] = null;
                    Count--;
                }
                break;
            case "Update":
                {
                    if (item.Count > 1)
                    {
                        Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Count.ToString();
                        Items[slot] = item;
                    }

                    if (item.Info.type == "Money")
                    {
                        Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Info.damage.ToString();
                    }
                }
                break;
            case "Clear":
                {
                    Count = 0;
                    uiName.text = UiName;
                    gameObject.SetActive(false);
                    for (int i = 0; i < 3; i++)
                    {
                        if (Items[i] != null)
                        {
                            buttons[i].interactable = true;
                        }
                        Slots[i].sprite = Resources.Load<Sprite>("TheNULL");
                        Slots[i].gameObject.transform.parent.gameObject.SetActive(false);
                    }
                    var rt = Slots[slot].gameObject.transform.parent.parent.gameObject.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(pxSize, rt.sizeDelta.y);
                }
                break;
            default:
                break;
        }
    }
}
