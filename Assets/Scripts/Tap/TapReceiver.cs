using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    [SerializeField] private GameObject template;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnTap += this.OnTap;
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnTap -= this.OnTap;
    }

    public void OnTap(object sender, TapEventArgs args)
    {
        if(args.HitObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            
        }
    }
}
