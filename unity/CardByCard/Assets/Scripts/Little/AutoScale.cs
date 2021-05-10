using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    public Vector3 startScale;
    public float minSize;
    public float startDelay;

    [Range(0.001f, 0.05f)]
    public float speed = 0.02f;
    void Start()
    {
        StartCoroutine(scaleDown());
    }

    private IEnumerator scaleDown()
    {
        yield return new WaitForSeconds(startDelay);
        var x = startScale.x;
        var y = startScale.y;
        while (transform.localScale.x > minSize)
        {
            x = x - 0.008f;
            y = y - 0.008f;
            transform.localScale = new Vector3(x, y, 1);
            yield return new WaitForSeconds(speed);
        }
    }
}
