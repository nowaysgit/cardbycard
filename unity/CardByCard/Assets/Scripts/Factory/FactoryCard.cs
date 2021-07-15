using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class FactoryCard : FactoryBase
{
    [Header("Chance LVL Settings")]
    [SerializeField] public int[] LvlProgression;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] cardsPrefab;

    [Header("Chance Card Settings ( Enemy | Loot | Block | Empty | Shop )")]
    [SerializeField] private string[] typeCardName;
    [SerializeField] private int[] chanceCardProgression;
    [SerializeField] private int[] maxCardsTypeCount = { 6, 9, 3, 3, 4, }; // ( Enemy | Loot | Block | Empty | Shop )
    private int[] cardsTypeCount = { 0, 0, 0, 0, 0, };    // ( Enemy | Loot | Block | Empty | Shop )

    public void Make(int x, int y, Vector2 pos, int typeID = -1, int itemID = -1)
    {
        if (itemID != -1)
        {
            var loot = Game.Data.LootList[itemID]; // Minus not spawned element they was last
            Spawn(x, y, pos, loot, 1);
            return;
        }
        while (typeID == -1)
        {
            typeID = RandomRules(chanceCardProgression);
            if (CheckRules(typeID) == false)
            {
                typeID = -1;
            }
        }
        InfoCard infoCard = MakeInfo(typeID);
        while (!IsSpawned(infoCard))
        {
            infoCard = MakeInfo(typeID);
        }
        Spawn(x, y, pos, infoCard, typeID);
    }

    public InfoCard MakeInfo(int typeID)
    {
        var list = (InfoCard[]) ((Game.Data.GetType()).GetProperty(typeCardName[typeID])).GetValue(Game.Data, null);
        bool find = false;
        while (!find)
        {
            var cardLvl = RandomRules(LvlProgression);
            foreach (var card in list)
            {
                if (card.lvl == cardLvl) {
                    find = true; 
                    return card;
                }
            }
        }
        return Game.Data.EmptyList[0];
    }
    private void Spawn(int x, int y, Vector2 pos, InfoCard infoCard, int typeID = -1)
    {
        if (Game.ControllerField.Field[x, y]) GameObject.Destroy(Game.ControllerField.Field[x, y].gameObject);
        var card = GameObject.Instantiate(cardsPrefab[typeID], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        Game.ControllerField.Field[x, y] = card.GetComponent<CardBase>();
        Game.ControllerField.Field[x, y].Load(infoCard, x, y);
    }
    private bool IsSpawned(InfoCard infoCard)
    {
        if (infoCard.spawned) return true;
        return false;
    }
    private bool CheckRules(int typeId)
    {
        if (cardsTypeCount[typeId] > maxCardsTypeCount[typeId])
        {
            return false;
        }
        return true;
    }
}
