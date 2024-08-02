using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TappedEffectManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ChoiceDialogueTrigger choiceDialogueTrigger; 

    [Header("GameObjects to Enable")]
    [SerializeField] private List<GameObject> objectsToEnable;

    [Header("GameObjects to Destroy")]
    [SerializeField] private List<GameObject> objectsToDestroy;

    private void Start()
    {

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (choiceDialogueTrigger.HasBeenTapped())
        {
            EnableGameObjects();
            DestroyGameObjects();
        }
    }

    private void EnableGameObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }


        objectsToEnable.Clear();
    }

    private void DestroyGameObjects()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }


        objectsToDestroy.Clear();
    }
}
