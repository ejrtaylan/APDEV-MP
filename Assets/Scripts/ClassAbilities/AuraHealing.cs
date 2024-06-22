using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aura Healing", menuName = "Class Ability/Aura Healing")]
public class AuraHealing : ClassAbility
{
    
    public override void OnUse(){
        Debug.Log("Used Aura Healing");
    }
}
