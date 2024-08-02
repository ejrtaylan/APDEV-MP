using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;



    private bool playerInRange;
    private bool isTapped;
    private bool hasBeenTapped;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        hasBeenTapped = false;
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (isTapped)
            {
                ChoiceDialogueManager dialogueManager = ChoiceDialogueManager.GetInstance();
                dialogueManager.EnterDialogueMode(inkJSON);
                isTapped = false;
                hasBeenTapped = true;
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

    public bool HasBeenTapped()
    {
        return hasBeenTapped;
    }
}
