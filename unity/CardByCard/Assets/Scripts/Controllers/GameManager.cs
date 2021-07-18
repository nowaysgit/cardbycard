using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ControllerField))]
[RequireComponent(typeof(FactoryItem))]
[RequireComponent(typeof(FactoryCard))]
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(UIAnimator))]

[System.Serializable]
public class IntEvent : UnityEvent<int> {}
[System.Serializable]
public class FloatEvent : UnityEvent<float> {}
[System.Serializable]
public class StringEvent : UnityEvent<string> {}

public class GameManager : MonoBehaviour
{ 
    [Header("Player")]
    public ControllerPlayer Player;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Data")]
    [SerializeField] private TextAsset jsonFile;
    private bool isInitialized;
    
    [SerializeField] public static Data Data;
    public static ControllerField ControllerField { get; private set; }
    public static FactoryItem FactoryItem { get; private set; }
    public static FactoryCard FactoryCard { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static UIAnimator UIAnimator { get; private set; }
    public static GameManager singletone { get; private set; }

    [Header("On Game Restart")]
    public UnityEvent OnGameRestart;


    [Header("On Game Preparation")]
    public UnityEvent OnGamePreparation;


    [Header("On Game End")]
    public UnityEvent OnGameEnd;


    [Header("On Player Respawn")]
    public UnityEvent OnPlayerRespawn;


    [Header("On Player Moved")]
    public UnityEvent OnPlayerMoved;


    [Header("On Player Money Spend")]
    public IntEvent OnPlayerMoneySpend;


    [Header("On Player Mana Spend")]
    public FloatEvent OnPlayerManaSpend;


    [Header("On Player Health Spend")]
    public FloatEvent OnPlayerHealthSpend;


    [Header("On Equipment Add")]
    public IntEvent OnEquipmentAdd;


    [Header("On Equipment Add")]
    public IntEvent OnInventoryAdd;
    


    [Header("On Card Click")]
    public UnityEvent OnCardClick;

    [Header("On Card Die")]
    public StringEvent OnCardDie;

    public GameState CurrentState { get; private set; }
    [SerializeField] public GameStateMainMenu GameStateMainMenu;
    [SerializeField] public GameStatePreparation GameStatePreparation;
    [SerializeField] public GameStateInGame GameStateInGame;

    private void Awake()
    {
        if (!singletone) 
        { 
            singletone = this; 
            DontDestroyOnLoad(this); 
            Initialize();
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }
    private void Initialize()
    {
        if(!isInitialized)
        {
            Data = JsonUtility.FromJson<Data>(jsonFile.text); // STATIC DATA
            Player = (Instantiate(playerPrefab, new Vector3(0, 0, -1), Quaternion.identity)).GetComponent<ControllerPlayer>();
            Player.Load(200, 100, 10.0f, "Character1");

            FactoryItem = gameObject.GetComponent<FactoryItem>();
            FactoryCard = gameObject.GetComponent<FactoryCard>();
            UIManager = gameObject.GetComponent<UIManager>();
            UIAnimator = gameObject.GetComponent<UIAnimator>();

            OnGameRestart.AddListener(UIManager.OnGameRestart);
            OnPlayerRespawn.AddListener(UIManager.OnPlayerRespawn);
            OnCardDie.AddListener(FactoryCard.OnCardDie);
            OnGameEnd.AddListener(FactoryCard.OnGameEnd);

            OnPlayerHealthSpend.AddListener(UIManager.UpdateHealthBar);
            OnPlayerManaSpend.AddListener(UIManager.UpdateManaBar);
            OnPlayerMoneySpend.AddListener(UIManager.UpdateMoney);

            CurrentState = GameStateMainMenu;
            CurrentState.Enter();
            isInitialized = true;
        }
    }
    public void NextState()
    {
        ChangeState(CurrentState.NextState);
    }
    public void ChangeState(GameState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
    public void PlayerRespawn() 
    {
        OnPlayerRespawn.Invoke();
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