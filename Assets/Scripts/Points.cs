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

    // Update is called once per frame
    void Update()
    {
        
    }
}
