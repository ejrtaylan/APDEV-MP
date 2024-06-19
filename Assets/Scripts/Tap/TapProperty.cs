using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TapProperty
{
    [SerializeField] private float _time = 0.7f;

    public float Time
    {
        get{return this._time;}
        set{this._time = value;}
    }

    [SerializeField] private float _maxDistance = 0.1f;

    public float MaxDistance
    {
        get {return this._maxDistance;}
        set {this._maxDistance = value;}
    }
}
