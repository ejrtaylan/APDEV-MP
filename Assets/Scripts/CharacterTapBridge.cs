using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTapBridge : MonoBehaviour, ITappable
{
    private Combatant combatant;
    private ChoiceDialogueTrigger dialogueTrigger;

    private void Awake(){
        this.combatant = GetComponent<Combatant>();
        this.dialogueTrigger = GetComponent<ChoiceDialogueTrigger>();
    }
    public void OnTap(TapEventArgs args){
        if(CombatManager.Instance.CombatActive && combatant != null) combatant.OnTap(args);
        else if (dialogueTrigger != null) dialogueTrigger.OnTap(args);
    }   
}
