using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    public float speed;
    void Start()
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("GameController").GetComponent<AnimatorUI>().MoveUI(gameObject, gameObject.transform.GetChild(Random.Range(0,8)).gameObject, speed));
    }
}
