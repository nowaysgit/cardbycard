using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Game = GameManager;

public class CardEnemy : CardBase
{
    [Header("Health Bar")]
    public Transform HealthBar;
    public override void OnAwake()
    {
        HealthBar.localScale = new Vector3(0.7f, 1f, 1f);
    }
    public override float Event(float getdamage) //Move if die else get and give damage
    {
        if(!Alive) { return 0.0f; }
        SetDamage(getdamage);
        if (Health <= 0)
        { 
            return 0.0f;
        }
        else
        {
            return Damage;
        }
    }
    public override void SetDamage(float getdamage)
    {
        if(!Alive) return;
        base.SetDamage(getdamage);
        HealthBar.localScale = new Vector3((Health/HealthMax)*0.7f, 1f, 1f);
        //HealthBar.localScale = new Vector3((0.7f/HealthMax)*Health, 1f, 1f); 248 / 24
        if (Health <= 0)
        { 
            Die(); 
        }
    }
    public override void Die()
    {
        base.Die();
        Game.FactoryCard.Make(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y), 1, Info.lvl);
        Destroy(gameObject);
    }
}
