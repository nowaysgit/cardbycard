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

public abstract class CardBase : MonoBehaviour, ICard
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
    [Header("On Died")]
    public UnityEvent OnDied;
    
    public virtual void Awake()
    {
        Game.singletone.OnPlayerMoved.AddListener(isMoved);
        sprite = GetComponent<SpriteRenderer>();
        textHealth = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Instantiate(FxInstance, transform.position, Quaternion.identity);
        OnAwake();
        IsBlocked = false;

        EventTrigger trigger = GetComponent<EventTrigger>( );
		EventTrigger.Entry entry = new EventTrigger.Entry( );
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener( (data) => { Game.singletone.GameStateInGame.isCardClick( this.gameObject ); } );
		trigger.triggers.Add( entry );
    }
    public virtual void OnAwake() //overridden by heirs and called after Awake
    {
    }
    
    public void Load(InfoCard info, int x, int y)
    {
        Info = Game.Data.CopyFromSerialize<InfoCard>(info);
        possition = new Vector2Int (x, y); 
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = Info.title;
        sprite.sprite = Resources.Load<Sprite>(Info.spriteName);
    }

    public virtual void Event(float getdamage, out float givedamage, out bool canmove)
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = false; }
        givedamage = 0.0f;
        canmove = true;
    }
    public virtual bool Damage(float getdamage)
    {
        GameObject FxAttack = (GameObject)Instantiate(AttackFXPrefab) as GameObject;
        FxAttack.GetComponent<TextMesh>().text = Convert.ToString(getdamage);
        Info.health-= getdamage;
        textHealth.text = Convert.ToString(Info.health);
        return (Info.health > 0);
    }

    public virtual void Die()
    {
        IsBlocked = true;
        OnDied.Invoke();
        Game.singletone.OnCardDie.Invoke(Info.type);
        Instantiate(FxDie, transform.position, Quaternion.identity);
    }
    public virtual void isMoved() //overridden by heirs and called after player change position
    {
    }

}