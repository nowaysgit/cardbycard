using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 1.0f;
    void OnEnable()
    {
        StartCoroutine( Destroy() );
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

}
