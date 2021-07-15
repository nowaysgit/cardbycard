using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTeleport : CardBase
{
    public override void Event(float getdamage, out float givedamage, out bool canmove)
    {
        Debug.Log("TELEPORTED");
        canmove = true;
        givedamage = 0.0f;
    }
}
