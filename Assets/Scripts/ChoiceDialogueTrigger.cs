using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDialogueTrigger : MonoBehaviour, ITappable
{
    [Header("VisualCue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private bool isTapped;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (isTapped)
            {
                ChoiceDialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                isTapped = false;
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    public void OnTap(TapEventArgs args)
    {
        isTapped = true;
        Debug.Log("Tapped on: " + args.HitObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
