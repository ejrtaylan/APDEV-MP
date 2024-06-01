using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEventArgs : EventArgs
{
    private Vector3 _position;
    private GameObject _hitObject;

    public Vector3 Position
    {
        get {return this._position;}
    }

    public GameObject HitObject
    {
        get {return this._hitObject;}
    }

    public TapEventArgs (Vector2 position, GameObject hitObject)
    {
        this._position = position;
        this._hitObject = hitObject;
    }
}
