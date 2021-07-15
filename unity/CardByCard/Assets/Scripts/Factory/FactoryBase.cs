using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FactoryBase: MonoBehaviour
{
    protected int GetArraySum(int[] array, int id1, int id2)
    {
        var sum = 0;
        for (int i = id1; i < id2+1; i++)
        {
            sum+=array[i];
        }
        return sum;
    }
    protected int RandomRules(int[] chanceArray)
    {
        var rand = Random.Range(0, chanceArray.Sum());
        if (0 <= rand && rand < GetArraySum(chanceArray, 0, 0))
        {
            return 0;
        }
        for (int i = 1; i < chanceArray.Length; i++)
        {
            if (GetArraySum(chanceArray, 0, (i-1)) <= rand && rand < GetArraySum(chanceArray, 0, i))
            {
                return i;
            }
        }
        return 0;        
    }
}
