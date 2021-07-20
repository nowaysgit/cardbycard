using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class CardBlock : CardBase
{
    [Header("Health Bar")]
    public Transform HealthBar;

    public override void OnAwake()
    {
        HealthBar.localScale = new Vector3(0.7f, 1f, 1f);
    }
    public override float Event(float getdamage)
    {
        if (!Alive) { return 0.0f; }
        return 0.0f;
    }
    public override void SetDamage(float getdamage)
    {
        if(!Alive) return;
        Health -= getdamage;
        HealthBar.localScale = new Vector3((0.7f /HealthMax) * Health, 1f, 1f);
    }
    public override void isMoved()
    {
        SetDamage(1);
        if (Health <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        if(!Alive) return;
        base.Die();
        Game.FactoryCard.Make(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y));
        Destroy(gameObject);
    }
}
