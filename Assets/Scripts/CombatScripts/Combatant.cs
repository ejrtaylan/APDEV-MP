using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combatant : MonoBehaviour, IComparable
{
    [SerializeField] public ESubAreas CurrentArea;
    public ETeam CombatantTeam; 
    [SerializeField] public ClassStats CombatantClass; 
    [SerializeField] private int maxHealth;
    public List<ClassAbility> Abilities = new List<ClassAbility>();
    [SerializeField] private bool debugKill;

    [SerializeField] public CombatTile CurrentTile;

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

        if(this.CombatantTeam == ETeam.ENEMY_AI_TEAM){
            Abilities.Add(this.CombatantClass.Ability1);
            Abilities.Add(this.CombatantClass.Ability2);
        }
    }

    public void UpdateCurrentTile() {
        GameObject hitObject = null;

        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity, TileManager.Instance.tileMask, QueryTriggerInteraction.Ignore))
        {
            hitObject = hit.collider.gameObject;
        }

        if(hitObject == null) return;

        CombatTile tile = hitObject.GetComponent<CombatTile>();
        if(tile != null)
            this.CurrentTile = tile;

        if(this.CurrentTile != null)
            this.AlignToTile();
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

    public void FullHeal(){
        this.CurrentHealth = this.maxHealth;
    }

    public void Kill(){
        this.CurrentHealth = 0;
        this.gameObject.SetActive(false);
        CombatManager.Instance.RemoveCombatantFromCombat(this);
        CombatManager.Instance.TotalKilled++;
    }

    private void AlignToTile(){
        this.gameObject.transform.position = this.CurrentTile.gameObject.transform.position;
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
