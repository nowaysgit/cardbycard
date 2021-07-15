using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Game = GameManager;

public class ControllerField : ControllerBase
{
    [Header("Settings, fieldSize is = fieldSize*fieldSize")]
    [SerializeField] private int fieldSize;
    [SerializeField] private int cardSize;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Player Info")]
    public ControllerPlayer Player;
    public Vector2Int PlayerPossition = new Vector2Int(0, 0);
    public int NumMoves { get; private set; }

    [Header("On Player Moved")]
    public UnityEvent OnMoved;

    [Header("On Player Moved")]
    public UnityEvent OnCardClick;

    public GameObject LastCardClick;

    [Header("Flied")]
    public CardBase[,] Field;

    private void Start()
    {
        Field = new CardBase[fieldSize, fieldSize];
        NumMoves = 0;
        OnMoved.AddListener(IsMoved);
        GenerateField();
    }
    public void OnGameRestart()
    {
        Destroy(Player.gameObject);
        NumMoves = 0;
        PlayerPossition = new Vector2Int(0, 0);
        GenerateField();
    }
    public void OnPlayerRespawn()
    {
        Player.Load(100, 100, Player.Money/2, Player.Damage, "Character1");
    }
    public void TouchEventStart(Vector2 vector) // LEAN
    {
        var normalized = vector.normalized;
        Player.Control.Move(Convert.ToInt32(normalized.x), Convert.ToInt32(normalized.y));
    }
    private void IsMoved()
    {
        NumMoves++;
    }
    public void isCardClick( GameObject card ) // Called when player Click on card
	{
        LastCardClick = card;
		OnCardClick.Invoke();
	}
    private void ClearField()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int i2 = 0; i2 < fieldSize; i2++)
            {
                if (Field[i, i2] != null) Destroy(Field[i, i2].gameObject);
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
                    Player = (Instantiate(playerPrefab, new Vector3(x, y, -1), Quaternion.identity)).GetComponent<ControllerPlayer>();
                    Player.Load(100, 100, 0, 3.0f, "Character1");
                }
                else
                {
                    Game.FactoryCard.Make(i, i2, new Vector3(x, y, 0));
                }
                x += (cardSize + 0.2f);
            }
            y += (cardSize + 0.2f);
        }
    }
}
