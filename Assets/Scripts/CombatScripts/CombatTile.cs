using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTile : MonoBehaviour, ITappable
{
    [SerializeField] public bool Passable = true;
    public int x;
    public int y;

    public void OnTap(TapEventArgs args){
        StartCoroutine(CombatManager.Instance.processMovement(this));
    }
}
