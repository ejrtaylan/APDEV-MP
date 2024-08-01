
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClassAbility : ScriptableObject {

    public Combatant User;
    public Combatant Target;
    [SerializeField] protected EUnitActionTypes Action_Type;
    [SerializeField] private string Ability_Name;
    [SerializeField] private string Ability_Description;
    [SerializeField] protected int effect_lower_bound;
    [SerializeField] protected int effect_upper_bound;
    [SerializeField] protected int minimum_range;
    [SerializeField] protected int maximum_range;

    public EUnitActionTypes ActionType {
        get {return this.Action_Type;}
    }
    public string Name {
        get {return this.Ability_Name;}
    }
    public string Description {
        get {return this.Ability_Description;}
    }

    abstract public void OnUse();
}
