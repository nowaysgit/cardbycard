using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(Control))]
[RequireComponent(typeof(InventoryPlayer))]
[RequireComponent(typeof(InventoryBackpack))]

public class ControllerPlayer : Interactive
{
    public override float Health { get { return health; } set { health = value; Game.UIManager.TextHealth.text = Convert.ToString(Mathf.Round(health)); Game.UIManager.SliderHealth.value = health; } }
    public override float HealthMax { get { return healthMax; } set { healthMax = value; } }
    public override float Mana { get { return mana; } set { mana = value; Game.UIManager.TextMana.text = Convert.ToString(Mathf.Round(mana)); Game.UIManager.SliderMana.value = mana; } }
    public override float ManaMax { get { return manaMax; } set { manaMax = value; } }
    public int Money { get { return money; } set { money = value; Game.UIManager.TextMoney.text = Convert.ToString(money); } }
    public InventoryPlayer Inventory { get; private set; }
    public InventoryBackpack InventoryBackpack { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject attackFXPrefab;

    [HideInInspector]
    private int money = 0;
    private SpriteRenderer sprite;

    public Control Control; // FIX IT
    private bool isInitialized;
    private void Awake() 
    {
        Initialize();
    }
    private void Initialize()
    {
        if(!isInitialized)
        {
            Control = gameObject.GetComponent<Control>();
            Inventory = gameObject.GetComponent<InventoryPlayer>();
            InventoryBackpack = gameObject.GetComponent<InventoryBackpack>();
            isInitialized = true;
        }
    }
    private void Start()
    {
        Game.UIManager.UIBackpack.Load();
    }
    public void Load(int _health, int _mana, int _money, float _damage, string _spriteName)
    {
        Health = _health;
        Mana = _mana;
        Money = _money;
        Damage = _damage;
        Control.SpriteName = _spriteName;
        Alive = true;
    }
    public override void SetDamage(float _getdamage)
    {
        GameObject fxAttack = (GameObject)Instantiate(attackFXPrefab, transform.position, Quaternion.identity) as GameObject;
        if(!Alive) return;
        if (Shield > 0.00f) {
            float damaged = Convert.ToSingle(Math.Round(_getdamage*Shield, 2)); 
            Health = Convert.ToSingle(Math.Round(Health-(_getdamage-damaged), 2));  
            Mana = Convert.ToSingle(Math.Round(Mana-ManaCost, 2));
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString((_getdamage-damaged) +"("+ damaged +")");
        }
        else
        {
            Health-= _getdamage;  
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString(_getdamage);
        }
        if (Health<=0) 
        {
            Health = 0;
            Alive = false;
            OnDied.Invoke();
            Game.UIManager.DiePanel.SetActive(true);
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
    public void AbilityUse()
    {
        if(!Alive) return;
    }
}