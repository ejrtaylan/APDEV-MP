using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Class Ability/Attack")]
public class Attack : ClassAbility
{
    [SerializeField] private string Attack_Type;
    [SerializeField] private string Attack_Range;
    public string Type {
        get { return this.Attack_Type;}
    }
    public override void OnUse(){
        Debug.Log("Used Attack");

        int damage = Random.Range(this.effect_lower_bound, this.effect_upper_bound);
        switch(Attack_Type, Attack_Range){
            case ("Physical", "Melee"):
                damage += User.CombatantClass.StrMod;
                break;
            case ("Physical", "Ranged"):
                damage += User.CombatantClass.DexMod;
                break;
            case ("Magic", "Melee"):
                damage += User.CombatantClass.ChaMod;
                break;
            case ("Magic", "Ranged"):
                damage += User.CombatantClass.IntMod;
                break;
        }
        if(damage < 0) damage = 0;

        Animator animator = User.GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Attack");

        Instantiate(GameObject.Find("Damage"), new Vector3(Target.transform.position.x, Target.transform.position.y + 1.75f, Target.transform.position.z), Quaternion.identity);

        Target.Damage(Attack_Type, damage);
    }    
}
