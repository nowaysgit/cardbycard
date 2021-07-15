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

public class GameManager : MonoBehaviour
{ 
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
    public UnityEvent OnGameRestart;
    public UnityEvent OnPlayerRespawn;


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
            FactoryItem = gameObject.GetComponent<FactoryItem>();
            FactoryCard = gameObject.GetComponent<FactoryCard>();
            UIManager = gameObject.GetComponent<UIManager>();
            UIAnimator = gameObject.GetComponent<UIAnimator>();
            ControllerField = gameObject.GetComponent<ControllerField>();

            OnGameRestart.AddListener(ControllerField.OnGameRestart);
            OnGameRestart.AddListener(UIManager.OnGameRestart);

            OnPlayerRespawn.AddListener(ControllerField.OnPlayerRespawn);
            OnPlayerRespawn.AddListener(UIManager.OnPlayerRespawn);
            isInitialized = true;
        }
    }
    public void GameRestart() 
    {
        OnGameRestart.Invoke();
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