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
    public override void Event(float getdamage, out float givedamage, out bool canmove)
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = true; }
        canmove = false;
        givedamage = 0.0f;
    }
    public override bool Damage(float getdamage)
    {
        Info.health -= getdamage;
        HealthBar.localScale = new Vector3((0.7f/Info.maxHealth)*Info.health, 1f, 1f);
        return (Info.health <= 0);
    }
    public override void isMoved()
    {
        if (Damage(1)) Die();
    }
    public override void Die()
    {
        base.Die();
        Game.FactoryCard.Make(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y));
        Destroy(gameObject);
    }
}
