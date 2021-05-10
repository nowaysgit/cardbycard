using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    void Event(float getdamage, out float givedamage, out bool canmove);
    bool Damage(float getdamage);
    void Die();
}
