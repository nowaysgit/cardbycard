using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class RandomMove : MonoBehaviour
{
    public float speed;
    void Start()
    {
        StartCoroutine(Game.UIAnimator.MoveUI(gameObject, gameObject.transform.GetChild(Random.Range(0,8)).gameObject, speed));
    }
}
