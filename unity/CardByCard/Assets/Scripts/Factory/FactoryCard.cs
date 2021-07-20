using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class FactoryCard : FactoryBase
{
    [Header("Chance LVL Settings")]
    [SerializeField] public int[] LvlProgression;

    [Header("Chance Card Settings ( Enemy | Loot | Block | Empty | Shop )")]
    [SerializeField] private string[] typeCardName;
    [SerializeField] private Dictionary<string, int> typeCardId = new Dictionary<string, int> { {"Enemy", 0}, {"Loot", 1}, {"Block", 2}, {"Empty", 3}, {"Shop", 4}};
    [SerializeField] private int[] chanceCardProgression;
    [SerializeField] private int[] maxCardsTypeCount = { 6, 9, 1, 3, 0, }; // ( Enemy | Loot | Block | Empty | Shop )
    [SerializeField] private int[] cardsTypeCount = { 0, 0, 0, 0, 0, };    // ( Enemy | Loot | Block | Empty | Shop )

    public GameStateInGame GameStateInGame
    {
        get { return Game.singletone.GameStateInGame; }
    }
    public void Make(int x, int y, Vector2 pos, int typeID = -1, int itemID = -1)
    {
        if (itemID != -1)
        {
            var loot = Game.Data.LootList[itemID]; // Minus not spawned element they was last
            GameStateInGame.Spawn(x, y, pos, loot, 1);
            cardsTypeCount[typeID]++;
            return;
        }
        while (typeID == -1)
        {
            typeID = RandomRules(chanceCardProgression);
            if (CheckRules(typeID, x, y) == false)
            {
                typeID = -1;
            }
        }
        InfoCard infoCard = MakeInfo(typeID);
        while (!IsSpawned(infoCard))
        {
            infoCard = MakeInfo(typeID);
        }
        GameStateInGame.Spawn(x, y, pos, infoCard, typeID);
        cardsTypeCount[typeID]++;
    }
    public void OnCardDie(string type)
    {
        cardsTypeCount[typeCardId[type]]--;
    }
    public void OnGameEnd()
    {
        Array.Clear(cardsTypeCount, 0, cardsTypeCount.Length);
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
    private bool IsSpawned(InfoCard infoCard)
    {
        if (infoCard.spawned) return true;
        return false;
    }
    private bool CheckRules(int typeId, int x, int y)
    {
        if (cardsTypeCount[typeId] >= maxCardsTypeCount[typeId])
        {
            return false;
        }
        if (typeId == 2) // BLOCK CARD
        {
            return GameStateInGame.CanSpawnBlock(x, y);
        }
        return true;
    }
}
