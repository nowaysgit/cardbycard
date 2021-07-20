using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void UseWeapon(string abilityName, Item item, Interactive attacking = null, Interactive receiver = null)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(abilityName);
        theMethod.Invoke(this, new object[] { item, attacking, receiver });
    }
    public void Attack(Item item, Interactive attacking = null, Interactive receiver = null)
    {
        receiver.Health-=item.Info.damage;
    }
}
