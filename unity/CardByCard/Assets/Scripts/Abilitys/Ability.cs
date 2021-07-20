using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using UnityEngine;
using Game = GameManager;

public class InfoByAbility
{
    public InfoByAbility(Item item, Interactive attacking, Interactive receiver)
    {
        Attacking = attacking;
        Receiver = receiver;
        Item = item;
    }
    public Interactive Attacking;
    public Interactive Receiver;
    public Item Item;
}
public class Ability : MonoBehaviour
{
    private List<InfoByAbility> AbilityHistory = new List<InfoByAbility>();
    public void UseAbility(string abilityName, Item item, Interactive attacking = null, Interactive receiver = null)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(abilityName);
        theMethod.Invoke(this, new object[] { item, attacking, receiver });
    }
    public void DisposableHeal(Item item, Interactive attacking = null, Interactive receiver = null)
    {
        receiver.SetHealHealth(item.Info.damage);
    }
    public void FireBall(Item item, Interactive attacking = null, Interactive receiver = null)
    {
        receiver.SetDamage(item.Info.damage);
        attacking.SetMana(item.Info.manaCost);
    }
    public void SnowBall(Item item, Interactive attacking = null, Interactive receiver = null)
    {
        receiver.SetDamage(item.Info.damage);
        attacking.SetMana(item.Info.manaCost);
    }
    public void Shield(Item item, Interactive attacking = null, Interactive receiver = null)
    {
        if (!(AbilityHistory.Contains(new InfoByAbility(item, attacking, receiver))))
        {
            item.Count = item.Info.maxInStack;
            AbilityHistory.Add(new InfoByAbility(item, attacking, receiver));
        }
        if (item.IsClick)
        {
            Game.singletone.OnPlayerHealthSpend.AddListener(ShieldUpdate);
            attacking.Shield = item.Info.damage;
        }
        else
        {
            attacking.Shield = 0;
            Game.singletone.OnPlayerHealthSpend.RemoveListener(ShieldUpdate);
        }
        Game.singletone.OnAbilityTime.Invoke(AbilityHistory[0].Item.Slot, AbilityHistory[0].Item.Count);
    }
    public void ShieldUpdate(float health, float spended)
    {
        AbilityHistory[0].Item.Count--;
        Game.singletone.OnAbilityTime.Invoke(AbilityHistory[0].Item.Slot, AbilityHistory[0].Item.Count);
        if (AbilityHistory[0].Item.Count <= 0)
        {
            AbilityHistory[0].Item.Count = AbilityHistory[0].Item.Info.maxInStack;
            AbilityHistory[0].Attacking.Shield = 0;
            AbilityHistory[0].Item.Kd = AbilityHistory[0].Item.Info.kd;
            Game.singletone.OnPlayerHealthSpend.RemoveListener(ShieldUpdate);
            AbilityHistory[0].Item.IsClick = false;
            Game.singletone.OnAbilityClick.Invoke(AbilityHistory[0].Item.Slot);
        }
        else
        {
            AbilityHistory[0].Attacking.SetMana(AbilityHistory[0].Item.Info.manaCost);
        }
    }
}
