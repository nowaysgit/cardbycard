using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEmpty : CardBase
{
    public override float Event(float getdamage)
    {
        if(!Alive) { return 0.0f; }
        return 0.0f;
    }
    public override void OnLoad()
    {
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "";
    }
}
