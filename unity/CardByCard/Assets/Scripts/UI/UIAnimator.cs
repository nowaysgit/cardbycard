using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game = GameManager;

public class UIAnimator : MonoBehaviour
{
    public IEnumerator SetActiveDelay(GameObject obj, bool active,float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(active);
    }
    public IEnumerator MoveUI(GameObject obj, GameObject target, float speed)
    {
        yield return new WaitForSeconds(0.2f);
        while (obj && Vector3.Distance(obj.transform.position, target.transform.position) > 0.01f)
        {
            if(!obj) yield break;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.transform.position, Time.deltaTime * speed);

            yield return new WaitForSeconds(0.02f);
        }
    }
}
