using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Game = GameManager;

public class GameStateInGame : GameState
{
    [Header("Settings, fieldSize is = fieldSize*fieldSize")]
    [SerializeField] private int fieldSize;
    [SerializeField] private int cardSize;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Cards Prefabs")]
    [SerializeField] private GameObject[] cardsPrefab;

    [Header("Player Info")]
    public Vector2Int PlayerPossition = new Vector2Int(0, 0);
    public int NumMovesInGame { get; private set; }
    private int LocationDistance;

    public CardBase LastCardClick;

    [Header("Flied")]
    public CardBase[,] Field;

    private InfoLocation curretLocation;
    public override void Enter()
    {
        LocationDistance = Game.Map.curretLocation.Info.distance;
        Game.singletone.OnPlayerMoved.AddListener(IsMoved);
        Field = new CardBase[fieldSize, fieldSize];
        NumMovesInGame = 0;
        PlayerPossition = new Vector2Int(0, 0);
        GenerateField();

        Game.singletone.OnGameStart.Invoke();
        Game.singletone.OnPlayerRespawn.Invoke();
    }
    public override void Exit()
    {
        Game.singletone.OnPlayerMoved.RemoveListener(IsMoved);
        if (IsLocationEnd() && Game.FactoryCard.cardsTypeCount[0] <= 0 && Game.FactoryCard.cardsTypeCount[1] <= 1)
        {
            Game.singletone.OnCompliteLocation.Invoke();
        }
        Game.singletone.OnGameEnd.Invoke();
        NumMovesInGame = 0;
        ClearField();
    }
    public void PlayerRespawn()
    {
        Game.singletone.Player.Load(1000, 100, Game.singletone.Player.Damage, "Character1");
        Game.singletone.OnPlayerRespawn.Invoke();
    }
    public void TouchEventStart(Vector2 vector) // LEAN
    {
        var normalized = vector.normalized;
        Game.singletone.Player.Control.TryMove(Convert.ToInt32(normalized.x), Convert.ToInt32(normalized.y));
    }
    private void IsMoved()
    {
        if (IsLocationEnd())
        {
            return;
        }
        NumMovesInGame++;
    }
    public bool IsLocationEnd()
    {
        if (NumMovesInGame >= Game.Map.curretLocation.Info.distance)
        {
            return true;
        }
        return false;
    }
    public void isCardClick(CardBase card) // Called when player Click on card
    {
        LastCardClick = card;
        Game.singletone.OnCardClick.Invoke();
    }
    private void ClearField()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int i2 = 0; i2 < fieldSize; i2++)
            {
                if (Field[i, i2] != null)
                {
                    Destroy(Field[i, i2].gameObject);
                    Field[i, i2] = null;
                }
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
                if (PlayerPossition.x == i && PlayerPossition.y == i2)
                {
                    Game.FactoryCard.Make(i, i2, new Vector3(x, y, 0), 3);
                    if (Game.singletone.Player == null)
                    {
                        Game.singletone.Player = (Instantiate(playerPrefab, new Vector3(x, y, -1), Quaternion.identity)).GetComponent<ControllerPlayer>();
                        Game.singletone.Player.Load(100, 100, 3.0f, "Character1");
                        Game.singletone.OnPlayerRespawn.Invoke();
                    }
                    else
                    {
                        Game.singletone.Player.Load(Game.singletone.Player.HealthMax, Game.singletone.Player.ManaMax, Game.singletone.Player.Damage, "Character1");
                        Game.singletone.Player.Control.SetPossition(PlayerPossition.x, PlayerPossition.y);
                    }
                    var x123 = Field[i, i2].Event(0f);
                }
                else if (Field[i, i2] == null)
                {
                    Game.FactoryCard.Make(i, i2, new Vector3(x, y, 0));
                }
                x += (cardSize + 0.2f);
            }
            y += (cardSize + 0.2f);
        }
    }
    public void Spawn(int x, int y, Vector2 pos, InfoCard infoCard, int typeID = -1)
    {
        if (Game.singletone.CurrentState != Game.singletone.GameStateInGame)
        {
            return;
        }
        if (IsLocationEnd() && Game.FactoryCard.cardsTypeCount[0] <= 0 && Game.FactoryCard.cardsTypeCount[1] <= 1)
        {
            Game.singletone.NextState();
            return;
        }
        if (Field[x, y])
        { 
            GameObject.Destroy(Field[x, y].gameObject);
        }
        var card = GameObject.Instantiate(cardsPrefab[typeID], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        Field[x, y] = card.GetComponent<CardBase>();
        Field[x, y].Load(infoCard, x, y);
        Game.FactoryCard.cardsTypeCount[typeID]++;
    }
    public bool CanMove(int x, int y, Vector2Int possition, out float getdamage)
    {
        getdamage = 0;
        if (x != 0 && y != 0) return false;
        int tryPossitionX = possition.x + x;
        int tryPossitionY = possition.y + y;
        if ((x != 0 && tryPossitionX >= 0 && tryPossitionX < 3) || (y != 0 && tryPossitionY >= 0 && tryPossitionY < 3))
        {
            if (Field[tryPossitionY, tryPossitionX] == null) return false; // X AND Y REVERSED <WHY>
            var canmove = Field[tryPossitionY, tryPossitionX].Info.canMove;
            getdamage = Field[tryPossitionY, tryPossitionX].Event(Game.singletone.Player.Control.interactive.Damage); // X AND Y REVERSED <WHY>
            return canmove;
        }
        return false;
    }
    public void TryMakeNewCard(Vector2Int possition, Vector3 transformposition)
    {
        if (Field[possition.y, possition.x] != null)
        {
            if (Game.singletone.GameStateInGame.IsLocationEnd() && Field[possition.y, possition.x].Info.type == "Empty")
            {
                return;
            }
            Field[possition.y, possition.x].Die();
            Game.FactoryCard.Make(possition.y, possition.x, new Vector2(transformposition.x, transformposition.y));
        }
        else
        {
            Game.FactoryCard.Make(possition.y, possition.x, new Vector2(transformposition.x, transformposition.y));
        }
    }

    public bool CanSpawnBlock(int x, int y)
    {
        /*
        0,2  1,2  2,2
        0,1  1,1  2,1
        0,0  1,0  2,0  

        0,1 + 1,0 = 1,1 (2) LEFT DOWN
        0,1 + 1,2 = 1,3 (4) LEFT UP
        1,2 + 2,1 = 3,3 (6) RIGHT UP
        2,1 + 1,0 = 3,1 (4) RIGHT DOWN
        */
        if ((x == 0 && y == 1) && ((Field[x + 1, y - 1] && Field[x + 1, y - 1].Info.type == "Block") || (Field[x + 1, y + 1] && Field[x + 1, y + 1].Info.type == "Block"))) // 0,1 UP OR DOWN
        {
            return false;
        }
        else if ((x == 2 && y == 1) && ((Field[x - 1, y - 1] && Field[x - 1, y - 1].Info.type == "Block") || (Field[x - 1, y + 1] && Field[x - 1, y + 1].Info.type == "Block"))) // 2,1 UP OR DOWN
        {
            return false;
        }
        else if ((x == 1 && y == 0) && ((Field[x + 1, y + 1] && Field[x + 1, y + 1].Info.type == "Block") || (Field[x - 1, y + 1] && Field[x - 1, y + 1].Info.type == "Block"))) // 1,0 RIGHT OR LEFT
        {
            return false;
        }
        else if ((x == 1 && y == 2) && ((Field[x + 1, y - 1] && Field[x + 1, y - 1].Info.type == "Block") || (Field[x - 1, y - 1] && Field[x - 1, y - 1].Info.type == "Block"))) // 1,2 RIGHT OR LEFT
        {
            return false;
        }
        return true;
    }
}
