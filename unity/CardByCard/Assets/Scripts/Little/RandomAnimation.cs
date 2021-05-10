using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    public Animation animation;
    void Awake()
    {
        animation.Play("AnimCoinIco"+Random.Range(0,3).ToString());
    }
}
