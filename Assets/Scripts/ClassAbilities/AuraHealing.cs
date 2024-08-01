using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aura Healing", menuName = "Class Ability/Aura Healing")]
public class AuraHealing : ClassAbility
{
    public override void OnUse(){
        Debug.Log("Used Aura Healing");
        int healing = Random.Range(this.effect_lower_bound, this.effect_upper_bound) + User.CombatantClass.WisMod;
        if(healing < 0) healing = 0;

        List<Combatant> team = CombatManager.Instance.getTeamCombatants(User.CombatantTeam);
        foreach(Combatant ally in team){
            if(ally == User) continue;
            ally.Heal(healing);
        }
    }
}
