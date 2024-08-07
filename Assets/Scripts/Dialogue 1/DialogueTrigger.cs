using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Target Section and Choice Index for Dice Roll")]
    [SerializeField] private int targetStorySection;
    [SerializeField] private int targetChoiceIndex;

    [Header("Dice Roller Prefab")]
    [SerializeField] private GameObject DiceRoller;

    [Header("Difficulty Class")]
    [SerializeField] private int difficultyClass;

    [Header("Modifier")]
    [SerializeField] private int Modifier;

    [Header("Dialogue Panel")]
    [SerializeField] private GameObject DialoguePanel;

    [Header("Success and Failure JSON")]
    [SerializeField] private TextAsset successJSON;
    [SerializeField] private TextAsset failJSON;

    [Header("Keywords")]
    [SerializeField] private string[] keywords;

    private bool waitingForRoll = false;

    void Start()
    {
        this.DialoguePanel.SetActive(false);
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DICE_RESULT, this.AwaitRoll);
    }

    public void CheckForKeywords(string selectedChoiceText, int storySectionCounter, int choiceIndex)
    {
        Debug.Log("CheckForKeywords called with:");
        Debug.Log("Selected Choice Text: " + selectedChoiceText);
        Debug.Log("Story Section Counter: " + storySectionCounter);
        Debug.Log("Choice Index: " + choiceIndex);
        Debug.Log("Target Story Section: " + targetStorySection);
        Debug.Log("Target Choice Index: " + targetChoiceIndex);

        Debug.Log("Keywords List: " + string.Join(", ", keywords));

        // Check if the story section and choice index match the target
        if (storySectionCounter == targetStorySection && choiceIndex == targetChoiceIndex)
        {
            Debug.Log("Story section and choice index match the target.");

            // Check if the selected choice text contains any of the keywords
            foreach (string keyword in keywords)
            {
                Debug.Log("Checking for keyword: " + keyword);
                if (selectedChoiceText.Contains(keyword))
                {
                    Debug.Log("Keyword detected: " + keyword + " in Story Section: " + storySectionCounter + ", Choice Index: " + choiceIndex + ". Initiating dice roll...");

                    ChoiceDialogueManager dialogueManager = ChoiceDialogueManager.GetInstance();
                    if (dialogueManager != null)
                    {
                        dialogueManager.ExitDialogueMode();
                    }

                    this.DiceRoller.SetActive(true);
                    this.waitingForRoll = true;

                    Parameters param = new Parameters();
                    param.PutExtra("DIFFICULTY_CLASS", difficultyClass);
                    EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, param);

                    return;
                }
                else
                {
                    Debug.Log("Keyword not found: " + keyword);
                }
            }
            Debug.Log("No keywords matched.");
        }
        else
        {
            Debug.Log("Story section and choice index do not match the target.");
        }
    }


    public void AwaitRoll(Parameters param)
    {
        if (!this.waitingForRoll) return;

        this.waitingForRoll = false;
        this.DiceRoller.SetActive(false);

        bool rollResult = param.GetBoolExtra("ROLL_RESULT", false);
        Debug.Log("Result: " + rollResult);

        // Determine which JSON to load based on the roll result
        TextAsset chosenJSON = rollResult ? successJSON : failJSON;
        if (chosenJSON != null)
        {
            ChoiceDialogueManager dialogueManager = ChoiceDialogueManager.GetInstance();
            if (dialogueManager != null)
            {
                dialogueManager.EnterDialogueMode(chosenJSON);
            }
        }

        // If the result is a success, recruit the character if they have a Recruitable component
        if (rollResult)
        {
            Recruitable recruitable = GetComponent<Recruitable>();
            if (recruitable != null)
            {
                recruitable.RecruitCharacter();
            }
        }
    }
}
