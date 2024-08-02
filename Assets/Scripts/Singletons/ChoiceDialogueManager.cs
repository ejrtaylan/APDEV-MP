using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class ChoiceDialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button endButton; // End button to stop the conversation

    [Header("Disabled UI")]
    [SerializeField] private GameObject joystickPanel;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    [SerializeField] private GameObject choicePanel;

    private Story currentStory;
    private bool dialogueIsPlaying;

    private string currentStoryText;
    private List<string> currentChoicesText;
    private int storySectionCounter; // Add a counter to track story section

    private static ChoiceDialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than 1 Dialogue Manager in scene");
        }
        instance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        endButton.gameObject.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        endButton.onClick.AddListener(ExitDialogueMode);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
    }

    public static ChoiceDialogueManager GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        joystickPanel.SetActive(false);
        endButton.gameObject.SetActive(false);
        storySectionCounter = 0; // Reset the counter
        Debug.Log("Enter Dialogue Mode");

        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        joystickPanel.SetActive(true);
        endButton.gameObject.SetActive(false);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            currentStoryText = currentStory.Continue();
            dialogueText.text = currentStoryText;
            DisplayChoices();
            storySectionCounter++; // Increment the counter
        }
        else if (currentStory.currentChoices.Count == 0)
        {
            endButton.gameObject.SetActive(true);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        currentChoicesText = new List<string>();

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Please reconsider.");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            currentChoicesText.Add(choice.text);
            int choiceIndex = index;
            choices[index].GetComponent<Button>().onClick.RemoveAllListeners();
            choices[index].GetComponent<Button>().onClick.AddListener(() => OnChoiceButtonClicked(choiceIndex));
            index++;
        }

        for (int i = 0; i < choices.Length; i++)
        {
            if (i >= currentChoices.Count)
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        endButton.gameObject.SetActive(currentChoices.Count == 0);
    }

    private void OnChoiceButtonClicked(int choiceIndex)
    {
        TrackStoryAndChoices(choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void TrackStoryAndChoices(int choiceIndex)
    {
        // Log or store the current part of the story, choices, and the selected choice
        Debug.Log("Current Story Text: " + currentStoryText);
        Debug.Log("Current Choices: " + string.Join(", ", currentChoicesText));
        Debug.Log("Selected Choice: " + currentChoicesText[choiceIndex]);

        // Find the DialogueTrigger component
        DialogueTrigger dialogueTrigger = FindAnyObjectByType<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            // Pass the selected choice text to the CheckForKeywords method
            dialogueTrigger.CheckForKeywords(currentChoicesText[choiceIndex], storySectionCounter, choiceIndex);
            Debug.Log("Checking...");
        }
    }
}


