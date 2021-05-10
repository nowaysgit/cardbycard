using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;

public class EnemyCard : Card
{
    [Header("Health Bar")]
    public Transform HealthBar;
    public override void OnAwake()
    {
        HealthBar.localScale = new Vector3(0.7f, 1f, 1f);
    }
    public override void Event(float getdamage, out float givedamage, out bool canmove) //Move if die else get and give damage
    {
        if(IsBlocked) { givedamage = 0.0f; canmove = true; }
        var D = Damage(getdamage);
        if (!D) { givedamage = Info.damage; } else { givedamage = 0.0f; }
        canmove = D;
    }
    public override bool Damage(float getdamage)
    {
        if (IsBlocked) return true;
        Info.health -= getdamage;
        textHealth.text = Convert.ToString(Info.health);
        GameObject fxAttack = (GameObject)Instantiate(AttackFXPrefab, transform.position, Quaternion.identity) as GameObject;
        fxAttack.GetComponent<TextMesh>().text = Convert.ToString(getdamage);
        if (Info.health <= 0) { Die(); return false; }
        HealthBar.localScale = new Vector3((0.7f/Info.maxHealth)*Info.health, 1f, 1f);
        return false;
    }
    public override void Die()
    {
        IsBlocked = true;
        Instantiate(FxDie, transform.position, Quaternion.identity);
        game.MakeCard(possition.x, possition.y, new Vector2(transform.position.x, transform.position.y), 1, 1);
        Destroy(gameObject);
    }
}
