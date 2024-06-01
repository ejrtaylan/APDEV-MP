using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEventArgs : EventArgs
{
    private ESwipeDirection _direction;

    public ESwipeDirection Direction
    {
        get {return this._direction;}
    }

    public SwipeEventArgs (ESwipeDirection direction)
    {
        this._direction = direction;
    }
}
