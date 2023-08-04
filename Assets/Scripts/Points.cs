using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    public int hits;
    public int miss;
    // Start is called before the first frame update
    private void OnEnable()
    {
        hits = 0;
        miss = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
