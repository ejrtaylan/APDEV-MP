using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    [SerializeField] private GameObject Tapper;

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
        if(args != null)
        {
            if(args.HitObject == Tapper)
            {
                Debug.Log("Tap hit");
            }
        }
    }
}
