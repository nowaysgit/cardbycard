using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public abstract class Card : MonoBehaviour, ICard
{
    [Header("FX")]
    public GameObject FxInstance;
    public GameObject FxDie;
    public GameObject AttackFXPrefab;

    [Header("Info")]
    public CardInfo Info;

    [HideInInspector]
    public bool IsBlocked = true;

    
    protected Vector2Int possition = new Vector2Int (0,0);
    protected SpriteRenderer sprite;
    protected TextMeshPro textName;
    protected TextMeshPro textHealth;
    protected GameController game;

    
    public virtual void Awake()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sprite = GetComponent<SpriteRenderer>();
        textHealth = this.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Instantiate(FxInstance, transform.position, Quaternion.identity);
        OnAwake();
    }
    public virtual void OnAwake() //overridden by heirs and called after Awake
    {
    }
    public void Load(CardInfo _info, int x, int y)
    {
        Info = game.data.CopyFromSerialize<CardInfo>(_info);
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
        Instantiate(FxDie, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void isMoved() //overridden by heirs and called after player change position
    {
    }

}