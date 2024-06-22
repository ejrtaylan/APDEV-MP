using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedTapCube : MonoBehaviour, ITappable
{
    public void OnTap(TapEventArgs args)
    {
        Debug.Log("Capsule Tapped :)");
    }
}
