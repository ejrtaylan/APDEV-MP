using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatBlock : MonoBehaviour {
    [SerializeField] private int Strength_Score = 10;
    [SerializeField] private int Dexterity_Score = 10;
    [SerializeField] private int Constitution_Score = 10;
    [SerializeField] private int Wisdom_Score = 10;
    [SerializeField] private int Intelligence_Score = 10;
    [SerializeField] private int Charisma_Score = 10;

    public Stat STR;
    public Stat DEX;
    public Stat CON;
    public Stat WIS;
    public Stat INT;
    public Stat CHA;

    void Awake (){
        this.STR.Score = this.Strength_Score;
        this.DEX.Score = this.Dexterity_Score;
        this.CON.Score = this.Constitution_Score;
        this.WIS.Score = this.Wisdom_Score;
        this.INT.Score = this.Intelligence_Score;
        this.CHA.Score = this.Charisma_Score;
    }

    public void resetBonuses(){
        this.STR.Bonus = 0;
        this.DEX.Bonus = 0;
        this.CON.Bonus = 0;
        this.WIS.Bonus = 0;
        this.INT.Bonus = 0;
        this.CHA.Bonus = 0;
    }

    public void resetStats(){
        this.resetBonuses();

        this.STR.Score = 0;
        this.DEX.Score = 0;
        this.CON.Score = 0;
        this.WIS.Score = 0;
        this.INT.Score = 0;
        this.CHA.Score = 0;
    }
}
