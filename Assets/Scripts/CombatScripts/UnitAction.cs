using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour, ITappable
{
    [SerializeField] public ClassAbility classAbility;
    public void Perform(){

    }

    public void OnTap(TapEventArgs args){
        CombatManager.Instance.UnitActionTapped(this);
    }
}
