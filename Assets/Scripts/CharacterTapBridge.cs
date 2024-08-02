using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTapBridge : MonoBehaviour, ITappable
{
    private Combatant combatant;
    private ChoiceDialogueTrigger dialogueTrigger;

    private void Awake(){
        this.combatant = GetComponent<Combatant>();
    }
    public void OnTap(TapEventArgs args){
        if(CombatManager.Instance.CombatActive) combatant.OnTap(args);
        else dialogueTrigger.OnTap(args);

    }   
}
