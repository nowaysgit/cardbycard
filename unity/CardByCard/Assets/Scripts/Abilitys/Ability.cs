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
        var controller = attacking.GetComponent<Controller>();
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
    public void DisposableHealMana(Item item, GameObject attacking = null, GameObject receiver = null)
    {
        var controller = attacking.GetComponent<Controller>();
        var result = controller.Mana+item.Info.damage;
        var max = controller.ManaMax;
        if (result>=max) 
        {
            controller.Mana = max;
        }
        else
        {
            controller.Mana = result;
        }
    }
}
