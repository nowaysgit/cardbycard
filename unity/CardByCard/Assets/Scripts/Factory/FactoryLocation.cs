using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class FactoryLocation : FactoryBase
{
    [Header("Chance LVL Settings")]
    [SerializeField] public int[] LvlProgression;
    public InfoLocation MakeInfo(InfoLocation currentLocation)
    {
        InfoLocation location = Game.Data.LocationList[0];
        bool find = false;
        var locationLvl = RandomRules(LvlProgression);
        while (!find)
        {
            location = Game.Data.LocationList[Random.Range(0, 4)];
            if (location.lvl == locationLvl)
            {
                find = true;
                break;
            }
        }
        return location;
    }
}
