using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySwitchButton : MonoBehaviour, ITappable
{
    public void OnTap(TapEventArgs args){
        CombatManager.Instance.CyclePlayer();
    }
}
