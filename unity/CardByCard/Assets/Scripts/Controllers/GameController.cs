using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Header("Chance Card Settings | Enemy | Loot | Block | Empty | Shop")]
    public string[] typeCardName;
    public int[] chanceCardProgression;

    [Header("Chance LVL Settings")]
    public int[] lvlProgression;

    [Header("Settings")]
    public int fieldSize;
    public int cardSize;

    [Header("Prefabs")]
    public GameObject[] CardsPrefab;
    public GameObject PlayerPrefab;
    public GameObject ItemPrefab;

    [Header("Player Info")]
    public PlayerController player;
    public Vector2Int playerPossition = new Vector2Int(0, 0);
    public int numMoves;

    [Header("UI")]
    public Text textHealth;
    public Text textMana;
    public Text textMoney;
    public Slider sliderHealth;
    public Slider sliderMana;
    public GameObject diePanel;

    public AnimatorUI animatorUI;

    [Header("Inventory UI")]
    public InventoryUI inventoryUI;
    public InventoryUILoot inventoryUILoot;
    public InventoryUIItems inventoryUIItems;

    [Header("Data")]
    public TextAsset jsonFile;
    public Data data;

    [Header("Flied")]
    public Card[,] field;

    public void TouchEventStart(Vector2 vector) // LEAN
    {
        var normalized = vector.normalized;
        player.Move(Convert.ToInt32(normalized.x), Convert.ToInt32(normalized.y));
    }

    private void Awake()
    {
        numMoves = 0;
        field = new Card[fieldSize, fieldSize];
        data = new Data();
        data = JsonUtility.FromJson<Data>(jsonFile.text);
        GenerateField();
    }
    public void isMoved()
    {
        numMoves++;
        for (int i = 0; i < fieldSize; i++)
        {
            for (int i2 = 0; i2 < fieldSize; i2++)
            {
                if (field[i, i2] != null) field[i, i2].isMoved();
            }
        }
    }
    public void Restart()
    {
        Destroy(player.gameObject);
        inventoryUILoot.UpdateInventory("Clear");
        inventoryUI.UpdateInventory("Clear");
        numMoves = 0;
        playerPossition = new Vector2Int(0, 0);
        GenerateField();
        inventoryUIItems.ReloadAll();
        diePanel.SetActive(false);
    }
    public void Respawn()
    {
        player.Load(100, 100, player.Money/2, player.Damage);
        diePanel.SetActive(false);
    }
    private void ClearField()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int i2 = 0; i2 < fieldSize; i2++)
            {
                if (field[i, i2] != null) Destroy(field[i, i2].gameObject);
            }
        }
    }
    private void GenerateField()
    {
        ClearField();
        var x = 0.0f;
        var y = 0.0f;
        for (int i = 0; i < fieldSize; i++)
        {
            x = 0.0f;
            for (int i2 = 0; i2 < fieldSize; i2++)
            {
                if (playerPossition.x == i && playerPossition.y == i2)
                {
                    player = (Instantiate(PlayerPrefab, new Vector3(x, y, -1), Quaternion.identity)).GetComponent<PlayerController>();
                    player.Load(100, 100, 0, 3.0f);
                    player.inventory = player.gameObject.GetComponent<InventoryPlayer>();
                }
                else
                {
                    MakeCard(i, i2, new Vector3(x, y, 0), RandomRules(chanceCardProgression));
                }
                x += (cardSize + 0.2f);
            }
            y += (cardSize + 0.2f);
        }
    }
    public bool CheckWall(int x, int y)
    {
        if (x == 0 && y == 1)
        {
            if ((field[1, 2].Info.type == "Block") || (field[1, 0].Info.type == "Block") || (field[1, 1].Info.type == "Block" && field[2, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 1 && y == 2)
        {
            if ((field[0, 1].Info.type == "Block") || (field[2, 1].Info.type == "Block") || (field[1, 1].Info.type == "Block" && field[1, 0].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 2 && y == 1)
        {
            if ((field[1, 2].Info.type == "Block") || (field[1, 0].Info.type == "Block") || (field[1, 1].Info.type == "Block" && field[0, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 1 && y == 0)
        {
            if ((field[0, 1].Info.type == "Block") || (field[2, 1].Info.type == "Block") || (field[1, 1].Info.type == "Block" && field[1, 2].Info.type == "Block"))
            {
                return false;
            }
        }


        else if (x == 1 && y == 1)
        {
            if ((field[1, 2].Info.type == "Block" && field[1, 0].Info.type == "Block") || (field[0, 1].Info.type == "Block" && field[2, 1].Info.type == "Block"))
            {
                return false;
            }
        }


        else if (x == 1 && y == 1)
        {
            if (field[0, 2].Info.type == "Block" && field[2, 2].Info.type == "Block" && field[1, 0].Info.type == "Block")
            {
                return false;
            }
        }
        else if (x == 1 && y == 1)
        {
            if (field[0, 1].Info.type == "Block" && field[2, 2].Info.type == "Block" && field[2, 0].Info.type == "Block")
            {
                return false;
            }
        }
        else if (x == 1 && y == 1)
        {
            if (field[1, 2].Info.type == "Block" && field[0, 0].Info.type == "Block" && field[2, 0].Info.type == "Block")
            {
                return false;
            }
        }
        else if (x == 1 && y == 1)
        {
            if (field[2, 1].Info.type == "Block" && field[0, 0].Info.type == "Block" && field[0, 2].Info.type == "Block")
            {
                return false;
            }
        }


        else if (x == 2 && y == 1)
        {
            if ((field[0, 0].Info.type == "Block" && field[1, 0].Info.type == "Block") || (field[0, 0].Info.type == "Block" && field[1, 1].Info.type == "Block") || (field[0, 2].Info.type == "Block" && field[1, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 2 && y == 2)
        {
            if ((field[0, 2].Info.type == "Block" && field[1, 1].Info.type == "Block") || (field[0, 0].Info.type == "Block" && field[1, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 0 && y == 2)
        {
            if ((field[0, 0].Info.type == "Block" && field[1, 1].Info.type == "Block") || (field[2, 0].Info.type == "Block" && field[1, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        else if (x == 0 && y == 0)
        {
            if ((field[2, 0].Info.type == "Block" && field[1, 1].Info.type == "Block") || (field[2, 2].Info.type == "Block" && field[1, 1].Info.type == "Block"))
            {
                return false;
            }
        }
        return true;
    }
    public int RandomRules(int[] chanceArray)
    {
        var rand = Random.Range(0, chanceArray.Sum());
        if (0 <= rand && rand < getArraySum(chanceArray, 0, 0))
        {
            return 0;
        }
        for (int i = 1; i < chanceArray.Length; i++)
        {
            if (getArraySum(chanceArray, 0, (i-1)) <= rand && rand < getArraySum(chanceArray, 0, i))
            {
                return i;
            }
        }
        Debug.Log("LVL ERROR: 0");
        return 0;        
    }

    //CARD FUNCTIONS
    private int getArraySum(int[] array, int id1, int id2)
    {
        var sum = 0;
        for (int i = id1; i < id2+1; i++)
        {
            sum+=array[i];
        }
        return sum;
    }
    private CardInfo MakeCardInfo(int typeID)
    {
        var list = (CardInfo[]) (data.GetType()).GetProperty(typeCardName[typeID]).GetValue(data, null);
        bool find = false;
        while (!find)
        {
            var cardLvl = RandomRules(lvlProgression);
            foreach (var card in list)
            {
                if (card.lvl == cardLvl) {
                    find = true; 
                    return card;
                }
            }
        }
        return data.EmptyList[0];
    }
    public void MakeCard(int x, int y, Vector2 pos, int typeID = -1, int itemID = -1)
    {
        if (itemID != -1)
        {
            var loot = data.LootList[itemID]; // Minus not spawned element they was last
            StartCoroutine(SpawnCard(x, y, pos, loot, 1));
            return;
        }
        while (typeID == -1)
        {
            typeID = RandomRules(chanceCardProgression);
            if (typeID == 2)
            {
                if (CheckWall(x, y) == false)
                {
                    typeID = -1;
                }
            }
        }
        CardInfo cardInfo = MakeCardInfo(typeID);
        while (!isSpawned(cardInfo))
        {
            cardInfo = MakeCardInfo(typeID);
        }
        StartCoroutine(SpawnCard(x, y, pos, cardInfo, typeID));
    }
    private bool isSpawned(CardInfo cardInfo)
    {
        if (cardInfo.spawned) return true;
        return false;
    }
    private IEnumerator SpawnCard(int x, int y, Vector2 pos, CardInfo cardInfo, int typeID = -1)
    {
        if (field[x, y]) Destroy(field[x, y].gameObject);
        var card = Instantiate(CardsPrefab[typeID], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        field[x, y] = card.GetComponent<Card>();
        field[x, y].gameObject.SetActive(false);
        field[x, y].Load(cardInfo, x, y);

        yield return new WaitForSeconds(0.5f);
        if (field[x, y] != null) field[x, y].gameObject.SetActive(true);
        field[x, y].IsBlocked = false;
    }


    //ITEMS FUNCTIONS
    public ItemInfo RandomItemRules(string RulesType)
    {
        ItemInfo item;
        var rand = Random.Range(0, 100);
        switch (RulesType)
        {
            case "Corpse":
                {
                    if (rand < 60) // MONEY
                    {
                        item = data.ItemList[0];
                        item.cost = Random.Range(1, 16);
                    }
                    else if (rand < 95) // HALF HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = data.ItemList[2];
                        }
                        else item = data.ItemList[4];
                    }
                    else // FULL HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = data.ItemList[1];
                        }
                        else item = data.ItemList[3];
                    }
                    return item;
                }
            case "Shop":
                {
                    if (rand < 20) // HALF HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = data.ItemList[2];
                        }
                        else item = data.ItemList[4];
                    }
                    else if (rand < 30) // FULL HEAL
                    {
                        if (Random.Range(0, 3) <= 2)
                        {
                            item = data.ItemList[1];
                        }
                        else item = data.ItemList[3];
                    }
                    else// Weapons
                    {
                        item = data.ItemList[Random.Range(5, 8)];
                    }
                    return item;
                }
            default:
                {
                    item = data.ItemList[0];
                    return item;
                }
        }
    }
    public Item MakeItem(int itemID = -1, string RulesType = "Corpse")
    {
        ItemInfo itemInfo;
        if (itemID == -1) itemInfo = RandomItemRules(RulesType);
        else itemInfo = data.ItemList[itemID];

        return SpawnItem(itemInfo);
    }
    private Item SpawnItem(ItemInfo itemInfo)
    {
        var itemGameObject = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var item = itemGameObject.GetComponent<Item>();
        item.Load(itemInfo);
        return item;
    }



    //OTHER FUNCTIONS
    public Color ParseStringColor(string rgbaColorToParse)
    {
        string[] rgba = Regex.Split(rgbaColorToParse, ", ");
        float red = float.Parse(new String(rgba[0].Where(x => !char.IsLetter(x) && char.GetUnicodeCategory(x) != UnicodeCategory.OpenPunctuation).ToArray()), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        float green = float.Parse(new String(rgba[1].Where(x => !char.IsLetter(x)).ToArray()), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        float blue = float.Parse(new String(rgba[2].Where(x => !char.IsLetter(x)).ToArray()), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        float alpha = float.Parse(new String(rgba[3].Where(x => !char.IsLetter(x) && char.GetUnicodeCategory(x) != UnicodeCategory.ClosePunctuation).ToArray()), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

        Color color = new Color(red, green, blue, alpha);
        return color;
    }
}