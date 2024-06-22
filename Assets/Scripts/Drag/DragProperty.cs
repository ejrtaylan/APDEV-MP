using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class DragProperty
{
    [Tooltip("Minimum allowable time to be considered a drag.")]
    [SerializeField]
    private float _time = 0.8f;

    public float Time
    {
        get { return this._time; }
        set { this._time = value; }
    }

}

