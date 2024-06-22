using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour, ITappable
{
    [SerializeField] public TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialoguePanel;

    public string[] lines;
    [SerializeField] public float TextSpeed;

    private int index;
    private bool isTyping;
    private bool isFirstLine;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        textComponent.text = string.Empty;
        isFirstLine = true;
        dialoguePanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartDialogue()
    {
        index = 0;
        isFirstLine = false;
        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    public void OnTap(TapEventArgs args)
    {
        if (isFirstLine)
        {
            Debug.Log("Start Dialogue here.");
            StartDialogue();
        }
        else
        {
            if (textComponent.text == lines[index])
            {
                Debug.Log("Continue to Next Line.");
                NextLine();
            }
            else
            {
                Debug.Log("Finish current line instantly.");
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("Dialogue finished.");
            dialoguePanel.SetActive(false);

            isFirstLine = true; // Reset for new dialogue session
        }
    }
}
