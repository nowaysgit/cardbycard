using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

[RequireComponent(typeof(Control))]
[RequireComponent(typeof(InventoryPlayer))]
[RequireComponent(typeof(InventoryEquipment))]

public class ControllerPlayer : Interactive
{
    public override float Health
    {
        get { return health; }
        set
        {
            health = value;
        }

    }
    public override float Mana
    {
        get { return mana; }
        set
        {
            mana = value;
        }
    }
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            Game.singletone.OnPlayerMoneySpend.Invoke(money);
        }
    }
    public override float HealthMax { get { return healthMax; } set { healthMax = value; } }
    public override float ManaMax { get { return manaMax; } set { manaMax = value; } }
    public InventoryPlayer Inventory { get; private set; }
    public InventoryEquipment InventoryEquipment { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject attackFXPrefab;

    [HideInInspector]
    private int money = 300;
    public int NumMoves;
    private SpriteRenderer sprite;
    public string SpriteName { get; private set; }

    public Control Control; // FIX IT
    private bool isInitialized;
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (!isInitialized)
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
            Control = gameObject.GetComponent<Control>();
            Inventory = gameObject.GetComponent<InventoryPlayer>();
            InventoryEquipment = gameObject.GetComponent<InventoryEquipment>();
            isInitialized = true;
        }
    }
    private void Start() 
    {
        Game.singletone.OnPlayerRespawn.Invoke();
    }
    private void SetSprite(string _spritename)
    {
        sprite.sprite = Resources.Load<Sprite>(_spritename);
    }
    public void Load(float health, float mana, float damage, string spriteName)
    {
        gameObject.SetActive(true);
        Health = health;
        HealthMax = health;
        Mana = mana;
        ManaMax = mana;
        Damage = damage;
        SpriteName = spriteName;
        Alive = true;
    }
    public override void SetDamage(float getdamage)
    {
        GameObject fxAttack = (GameObject)Instantiate(attackFXPrefab, transform.position, Quaternion.identity) as GameObject;
        if (!Alive) return;
        if (Shield > 0.00f)
        {
            float damaged = Convert.ToSingle(Math.Round(getdamage * Shield, 2));
            Health = Convert.ToSingle(Math.Round(Health - (getdamage - damaged), 2));
            Game.singletone.OnPlayerHealthSpend.Invoke(Health, -(getdamage - damaged));
            Mana = Convert.ToSingle(Math.Round(Mana - ManaCost, 2));
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString((getdamage - damaged) + "(" + damaged + ")");
        }
        else
        {
            Health -= getdamage;
            Game.singletone.OnPlayerHealthSpend.Invoke(Health, -getdamage);
            fxAttack.GetComponent<TextMesh>().text = Convert.ToString(getdamage);
        }
        if (Health <= 0)
        {
            Health = 0;
            Alive = false;
            Game.singletone.OnPlayerHealthSpend.Invoke(0, -getdamage);
            OnDied.Invoke();
            Game.UIManager.DiePanel.SetActive(true);
        }
    }
    public override void SetMana(float getmana)
    {
        if(!Alive) return;
        if (Mana-getmana <= 0 )
        {
            Mana = 0;
        }
        else
        {
            Mana = Convert.ToSingle(Math.Round(Mana-getmana, 2));
            Game.singletone.OnPlayerManaSpend.Invoke(Mana, getmana);
        }     
        Game.singletone.OnPlayerManaSpend.Invoke(Mana, -getmana);   
    }
    public override void SetHealHealth(float getdamage)
    {
        if(!Alive) return;
        if (Health+getdamage >= HealthMax) 
        {
            Health = HealthMax;
        }
        else
        {
            Health+= getdamage;  
        }
        Game.singletone.OnPlayerHealthSpend.Invoke(Health, getdamage);
    }
    public override void SetHealMana(float getmana)
    {
        if(!Alive) return;
        if (Mana+getmana >= ManaMax) 
        {
            Mana = ManaMax;
        }
        else
        {
            Mana+= getmana;  
        }   
        Game.singletone.OnPlayerHealthSpend.Invoke(Health, getmana);   
    }
    public override void Die()
    {
        gameObject.SetActive(false);
        Alive = false;
    }
    public void AbilityUse()
    {
        if (!Alive) return;
    }
}