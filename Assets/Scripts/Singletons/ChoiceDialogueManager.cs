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
        endButton.gameObject.SetActive(false); // Hide the end button initially

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        endButton.onClick.AddListener(ExitDialogueMode); // Add listener to end button
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
        endButton.gameObject.SetActive(false); // Hide the end button when entering dialogue mode
        Debug.Log("Enter Dialogue Mode");

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        joystickPanel.SetActive(true);
        endButton.gameObject.SetActive(false); // Hide the end button when exiting dialogue mode
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else if (currentStory.currentChoices.Count == 0)
        {
            // If there are no choices, show the end button
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

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Please reconsider.");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            int choiceIndex = index; // Capture the current index in the loop
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
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
