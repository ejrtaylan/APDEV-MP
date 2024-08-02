using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Collider GameObject")]
    [SerializeField] private Collider colliderGameObject; 

    private bool playerInRange;
    private bool isTapped;
    private bool hasBeenTapped;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        hasBeenTapped = false;

        if (colliderGameObject == null)
        {
            Debug.LogError("Collider GameObject is not assigned!");
        }
        else
        {

            colliderGameObject.isTrigger = true;
        }
    }

    private void Start()
    {

        if (colliderGameObject != null)
        {
            colliderGameObject.isTrigger = true;
        }
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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited range.");
        }
    }

    public void OnTap(TapEventArgs args)
    {
        isTapped = true;
        Debug.Log("Tapped on: " + args.HitObject.name);
    }

    public bool HasBeenTapped()
    {
        return hasBeenTapped;
    }
}
