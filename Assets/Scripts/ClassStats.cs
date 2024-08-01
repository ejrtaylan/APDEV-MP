using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Class Stats")]
public class ClassStats : ScriptableObject {
    [SerializeField] private string Class_Name;
    [SerializeField] private int Strength_Score;
    [SerializeField] private int Dexterity_Score;
    [SerializeField] private int Constitution_Score;
    [SerializeField] private int Wisdom_Score;
    [SerializeField] private int Intelligence_Score;
    [SerializeField] private int Charisma_Score;
    [SerializeField] private ClassAbility ability1;
    [SerializeField] private ClassAbility ability2;

    public string Name {
        get { return this.Class_Name;}
    }
    public int StrMod {
        get {return (this.Strength_Score - 10) / 2;}
    }
    public int DexMod {
        get {return (this.Dexterity_Score - 10) / 2;}
    }    
    public int ConMod {
        get {return (this.Constitution_Score - 10) / 2;}
    }    
    public int IntMod {
        get {return (this.Intelligence_Score - 10) / 2;}
    }    
    public int WisMod {
        get {return (this.Wisdom_Score - 10) / 2;}
    }    
    public int ChaMod {
        get {return (this.Charisma_Score - 10) / 2;}
    }    

    public ClassAbility Ability1 {
        get {return (this.ability1);}
    }
    public ClassAbility Ability2 {
        get {return (this.ability2);}
    }
}
