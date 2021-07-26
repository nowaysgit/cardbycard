using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Random = UnityEngine.Random;
using Game = GameManager;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EventTrigger))]

public abstract class CardBase : Interactive, ICard
{
    [Header("FX")]
    public GameObject FxInstance;
    public GameObject FxDie;
    public GameObject AttackFXPrefab;

    [Header("Info")]
    public InfoCard Info;

    [HideInInspector]
    public bool IsBlocked = true;

    
    protected Vector2Int possition = new Vector2Int (0,0);
    protected SpriteRenderer sprite;
    protected TextMeshPro textName;
    protected TextMeshPro textHealth;
    
    public virtual void Awake()
    {
        Game.singletone.OnPlayerMoved.AddListener(isMoved);
        sprite = GetComponent<SpriteRenderer>();
        textHealth = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Instantiate(FxInstance, transform.position, Quaternion.identity);
        OnAwake();
        Alive = true;

        EventTrigger trigger = GetComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener( (data) => { Game.singletone.GameStateInGame.isCardClick( this ); } );
		trigger.triggers.Add( entry );
    }
    public virtual void OnAwake() //overridden by heirs and called after Awake
    {
    }
    
    public void Load(InfoCard info, int x, int y)
    {
        Info = Game.Data.CopyFromSerialize<InfoCard>(info);
        possition = new Vector2Int (x, y); 

        HealthMax = Info.maxHealth;
        ManaMax = Info.maxMana;
        Mana = Info.mana;
        Health = Info.health;
        Damage = Info.damage;
        UniqueLvl = info.lvl;

        this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = (UniqueLvl+1).ToString();
        sprite.sprite = Resources.Load<Sprite>(Info.spriteName);
        OnLoad();
    }
    public virtual void OnLoad() //overridden by heirs and called after Load
    {
    }

    public virtual float Event(float getdamage) //RETURN DAMAGE
    {
        if(!Alive) { return 0.0f; }
        if (Health <= 0) 
        { 
            Die(); 
        }
        return 0.0f;
    }
    public override void SetDamage(float getdamage)
    {
        if(!Alive) return;
        Health -= getdamage;
        textHealth.text = Convert.ToString(Health);

        GameObject fxAttack = (GameObject)Instantiate(AttackFXPrefab, transform.position, Quaternion.identity) as GameObject;
        fxAttack.GetComponent<TextMesh>().text = Convert.ToString(getdamage);
    }

    public override void Die()
    {
        if(!Alive) return;
        Alive = false;
        OnDied.Invoke();
        Instantiate(FxDie, transform.position, Quaternion.identity);
    }
    private void OnDestroy() 
    {
        Game.singletone.OnCardDie.Invoke(Info.type);
    }
    public virtual void isMoved() //overridden by heirs and called after player change position
    {
    }

}