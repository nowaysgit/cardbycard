using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTeleport : CardBase
{
    public override float Event(float getdamage)
    {
        Debug.Log("TELEPORTED");
        return 0.0f;
    }
}
