using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public float Event(float getdamage);
    public void SetDamage(float getdamage);
    public void Die();
}
