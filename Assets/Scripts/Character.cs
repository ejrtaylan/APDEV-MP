using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private ClassStats classStats;

    public ClassStats Class{
        get { return this.classStats; }
    }

    private void Start() {
        
    }
}
