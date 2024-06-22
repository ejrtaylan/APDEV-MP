using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Class Ability/Attack")]
public class Attack : ClassAbility
{
    [SerializeField] private string Attack_Type;
    public string Type {
        get { return this.Attack_Type;}
    }
    public override void OnUse(){
        Debug.Log("Used Attack");
    }
}
