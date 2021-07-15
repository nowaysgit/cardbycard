using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public void UseAbility(string abilityName, Item item, GameObject attacking = null, GameObject receiver = null)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(abilityName);
        theMethod.Invoke(this, new object[] { item, attacking, receiver });
    }
    public void DisposableHeal(Item item, GameObject attacking = null, GameObject receiver = null)
    {
        var controller = attacking.GetComponent<Interactive>();
        var result = controller.Health+item.Info.damage;
        var max = controller.HealthMax;
        if (result>=max) 
        {
            controller.Health = max;
        }
        else
        {
            controller.Health = result;
        }
    }
    public void FireBall(Item item, GameObject attacking = null, GameObject receiver = null)
    {
        var rec = receiver.GetComponent<CardBase>();
        var att = attacking.GetComponent<Interactive>();

        rec.Damage(item.Info.damage);
        att.SetMana(item.Info.manaCost);
    }
    public void SnowBall(Item item, GameObject attacking = null, GameObject receiver = null)
    {
        var rec = receiver.GetComponent<CardBase>();
        var att = attacking.GetComponent<Interactive>();

        rec.Damage(item.Info.damage);
        att.SetMana(item.Info.manaCost);
    }
}
