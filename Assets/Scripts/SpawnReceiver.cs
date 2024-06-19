using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReceiver : MonoBehaviour, ITappable, ISwipeable
{
    public void OnTap(TapEventArgs args)
    {
        Debug.Log("SpawnReceiver OnTap");
        Destroy(this.gameObject);
    }

    public void OnSwipe(SwipeEventArgs args)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
