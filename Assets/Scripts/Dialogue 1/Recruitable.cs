using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recruitable : MonoBehaviour
{
    [Header("Character GameObject")]
    [SerializeField] private GameObject character;

    private void Awake()
    {
        if (character == null)
        {
            Debug.LogError("Character GameObject is not assigned!");
        }
    }

    public void RecruitCharacter()
    {
        if (character != null)
        {
            CombatManager.Instance.AddAsPlayerAlly(character);
        }
        else
        {
            Debug.LogError("Character GameObject is null, cannot recruit!");
        }
    }
}
