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


    [Header("DialoguePanel")]
    [SerializeField] private GameObject DialoguePanel;

    private bool waitingForRoll = false;

    void Start()
    {
        this.DialoguePanel.SetActive(false);
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DICE_RESULT, this.AwaitRoll);
    }


    public void CheckForKeywords(int storySectionCounter, int choiceIndex)
    {
        if (storySectionCounter == targetStorySection && choiceIndex == targetChoiceIndex)
        {
            Debug.Log("Target section and choice index met: Story Section: " + storySectionCounter + ", Choice Index: " + choiceIndex + ". Initiating dice roll...");

            this.DiceRoller.SetActive(true);
            this.waitingForRoll = true;

            Parameters param = new Parameters();
            param.PutExtra("DIFFICULTY_CLASS", difficultyClass);
            EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, param);

        }
    }

    public void AwaitRoll(Parameters param)
    {
        if (!this.waitingForRoll) return;

        this.waitingForRoll = false;
        this.DiceRoller.SetActive(false);

        Debug.Log("Result: " + param.GetBoolExtra("ROLL_RESULT", false));

        ChoiceDialogueManager dialogueManager = ChoiceDialogueManager.GetInstance();
        dialogueManager.ExitDialogueMode();
    }
}
