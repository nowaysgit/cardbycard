using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game = GameManager;

public class UILoot : UIInventory
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject coinIcon;
    [SerializeField] private GameObject coinIconPrefab;

    [Header("Color Settings")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color itemColor;

    private Button[] buttons;
    private float pxSize = 40;
    private Text uiName;

    [Header("Items List")]
    public Item[] Items;
    public int Count;


    public override void Awake()
    {
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
    public override void OnClick(int slot = 0)
    {
        if (Items[slot] != null)
        {
            var item = Items[slot];
            if (item.Info.type == "Money") 
            {
                Game.singletone.Player.Money+=item.Info.cost;
            }
            else if (item.Info.type == "Disposable") 
            {
                Game.singletone.Player.Inventory.Add(item);
            }
            else 
            {
                Game.singletone.Player.InventoryEquipment.Add(item.Info);
            }
            UpdateInventory("Remove", slot);
        }
        if (Count <= 0) Close();
    }
    public void Close()
    {
        if(Game.singletone.GameStateInGame.IsLocationEnd() && Game.singletone.CurrentState == Game.singletone.GameStateInGame && Game.FactoryCard.cardsTypeCount[0] <= 0 && Game.FactoryCard.cardsTypeCount[1] <= 1)
        {
            Game.singletone.NextState();
        }
        gameObject.SetActive(false);
    }
    public override void UpdateInventory(string act, int slot = 0, Item item = null, string UiName = "Corpse")
    {
        switch (act)
        {
            case "Add":
                {
                    Count++;
                    gameObject.SetActive(true);
                    buttons[slot].interactable = true;
                    Slots[slot].gameObject.SetActive(true);
                    Slots[slot].gameObject.transform.parent.gameObject.GetComponent<Image>().color = itemColor;

                    Slots[slot].gameObject.transform.GetChild(0).GetComponent<Text>().text = item.Info.title.ToString();
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    Slots[slot].sprite = Resources.Load<Sprite>(item.Info.spriteName);
                    Items[slot] = item;
                    Slots[slot].gameObject.transform.parent.gameObject.SetActive(true);
                    var rt = Slots[slot].gameObject.transform.parent.parent.gameObject.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x + 220, rt.sizeDelta.y);

                    if (item.Info.type == "Money")
                    {
                        var coin = Instantiate(coinIconPrefab, Slots[slot].gameObject.transform.position, Quaternion.identity);

                        coin.transform.SetParent(coinIcon.transform.parent, true);
                        coin.transform.localScale = coinIcon.transform.localScale * 2;

                        coin.GetComponent<RectTransform>().pivot = coinIcon.transform.GetComponent<RectTransform>().pivot;
                        coin.transform.GetChild(0).GetComponent<Text>().text = (item.Info.cost).ToString();

                        Game.UIAnimator.StartCoroutine(Game.UIAnimator.MoveUI(coin, coinIcon, 15000f));

                        OnClick(slot);
                        Destroy(coin, 1.7f);
                    }
                }
                break;
            case "Remove":
                {
                    Slots[slot].sprite = Resources.Load<Sprite>("TheNULL");
                    Slots[slot].gameObject.transform.parent.gameObject.GetComponent<Image>().color = emptyColor;
                    Items[slot] = null;
                    Slots[slot].gameObject.transform.GetChild(0).gameObject.SetActive(false);
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