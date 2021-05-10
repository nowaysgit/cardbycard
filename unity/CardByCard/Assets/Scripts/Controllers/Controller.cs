using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected float health = 100.0f;
    protected float healthMax = 100.0f;
    protected float mana = 100.0f;
    protected float manaMax = 100.0f;
    public virtual float Health { get; set; }
    public virtual float HealthMax { get; set; }

    public virtual float Mana { get; set; }
    public virtual float ManaMax { get; set; }
}
