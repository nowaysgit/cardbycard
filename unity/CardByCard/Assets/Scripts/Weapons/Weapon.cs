using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void UseWeapon(string abilityName, Item item, GameObject attacking = null, GameObject receiver = null)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(abilityName);
        theMethod.Invoke(this, new object[] { item, attacking, receiver });
    }
    public void Attack(Item item, GameObject attacking = null, GameObject receiver = null)
    {
        receiver.GetComponent<Interactive>().Health-=item.Info.damage;
    }
}
