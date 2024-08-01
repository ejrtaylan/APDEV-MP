using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour, ITappable, IComparable
{
    public ETeam CombatantTeam; 
    [SerializeField] public ClassStats CombatantClass; 
    [SerializeField] private int maxHealth;
    public List<UnitAction> UnitActions;
    [SerializeField] private bool debugKill;

    private int debugInitiative;
    public int CurrentHealth {get; private set;} = 1;

    private void Start(){
        Debug.Log(this);
        Debug.Log(this.gameObject);
        Debug.Log(CombatManager.Instance);
        CombatManager.Instance.AddCombatant(this);    
        this.debugInitiative = UnityEngine.Random.Range(0, 10);
        this.maxHealth = 20 + this.CombatantClass.ConMod;
        if(this.maxHealth <= 0) this.maxHealth = 1; 
    }

    private void OnDisable(){
        CombatManager.Instance.RemoveCombatant(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(debugKill){
            debugKill = false;
            this.Kill();
        }   
    }

    public void Heal(int amount){
        this.CurrentHealth += amount;
        if(this.CurrentHealth > maxHealth)
            this.CurrentHealth = maxHealth;
    }

    public void Damage(string atkType, int value){
        switch(atkType){
            case "Physical":
                value -= this.CombatantClass.ConMod;
                break;
            case "Magic":
                value -= this.CombatantClass.ChaMod;
                break;
        }
        if(value < 0) value = 0;

        this.CurrentHealth -= value;
        if(this.CurrentHealth <= 0) 
            this.Kill();
    }

    public void Kill(){
        this.CurrentHealth = 0;
        this.gameObject.SetActive(false);
    }

    public void OnTap(TapEventArgs args){
        CombatManager.Instance.processTargeting(this);
    }

    public int Initiative(){
        return debugInitiative;
    }

    public int CompareTo(object obj){
        Combatant other = (Combatant) obj;
        return this.Initiative() - other.Initiative();
    }

}
