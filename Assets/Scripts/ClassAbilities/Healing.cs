using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing", menuName = "Class Ability/Healing")]
public class Healing : ClassAbility
{
    public override void OnUse(){
        Debug.Log("Used Healing Skill");
        int healing = Random.Range(this.effect_lower_bound, this.effect_upper_bound) + User.CombatantClass.WisMod;
        if(healing < 0) healing = 0;

        Target.Heal(healing);
    }
}
