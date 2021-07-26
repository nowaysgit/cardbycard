using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    protected float health = 100.0f;
    protected float healthMax = 100.0f;
    protected float mana = 100.0f;
    protected float manaMax = 100.0f;
    protected float damage = 1.0f;
    protected float shield = 0.0f;
    public virtual float Health { get { return health; } set { health = value; } }
    public virtual float HealthMax { get { return healthMax; } set { healthMax = value; } }

    public virtual float Mana { get { return mana; } set { mana = value; } }
    public virtual float ManaMax { get { return manaMax; } set { manaMax = value; } }
    public virtual float Damage { get { return damage; } set { damage = value; } }
    public virtual float Shield { get { return shield; } set { shield = value; } }
    public virtual bool Alive { get; protected set; }
    public virtual int UniqueLvl { get; protected set; }

    public float ManaCost = 0.0f;

    [Header("On Died")] 
    public UnityEvent OnDied;

    public virtual void SetDamage(float getdamage)
    {
        if(!Alive) return;
        if (Shield > 0.00f) {
            float damaged = Convert.ToSingle(Math.Round(getdamage*Shield, 2)); 
            Health = Convert.ToSingle(Math.Round(Health-(getdamage-damaged), 2));  
            Mana = Convert.ToSingle(Math.Round(Mana-ManaCost, 2));
        }
        else
        {
            Health-= getdamage;  
        }
        if (Health<=0) 
        {
            Health = 0;
            Alive = false;
        }
    }
    public virtual void SetMana(float getmana)
    {
        if(!Alive) return;
        if (Mana-getmana <= 0 )
        {
            Mana = 0;
        }
        else
        {
            Mana = Convert.ToSingle(Math.Round(Mana-getmana, 2));
        }        
    }
    public virtual void SetHealHealth(float getdamage)
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
    }
    public virtual void SetHealMana(float getmana)
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
    }
    public virtual void Die()
    {
        if(!Alive) return;
        Destroy(gameObject);
    }
}
