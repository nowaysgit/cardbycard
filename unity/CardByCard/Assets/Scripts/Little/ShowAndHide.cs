using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAndHide : MonoBehaviour
{
    [SerializeField] private Text text;
    [Range(1, 10)]
    [SerializeField] private int delay;
    private float delayNow;

    public void Run(float health, float spended)
    {
        if (spended > 0)
        {
            text.text = "+" + spended.ToString();
        }
        else
        {
            text.text = spended.ToString();
        }
        if (delayNow > 0)
        {
            delayNow = delayNow + delay;
        }
        else
        {
            delayNow = delay;
            StartCoroutine(scaleDown());
        }
    }

    private IEnumerator scaleDown()
    {
        delayNow--;
        yield return new WaitForSeconds(0.1f);
        if (delayNow > 0)
        {
            StartCoroutine(scaleDown());
        }
        else
        {
            text.text = "";
        }
    }
}
