using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing", menuName = "Class Ability/Healing")]
public class Healing : ClassAbility
{
    
    public override void OnUse(){
        Debug.Log("Used Healing");
    }
}
