using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClassAbility : ScriptableObject {
    [SerializeField] private string Ability_Name;
    [SerializeField] private string Ability_Description;
    [SerializeField] private int effect_lower_bound;
    [SerializeField] private int effect_upper_bound;
    [SerializeField] private int minimum_range;
    [SerializeField] private int maximum_range;

    public string Name {
        get {return this.Ability_Name;}
    }
    public string Description {
        get {return this.Ability_Description;}
    }

    abstract public void OnUse();
}
