using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour, ITappable
{
    [SerializeField] public TextMeshProUGUI textComponent;
    public string[] lines;
    [SerializeField] public float TextSpeed;


    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    public void OnTap(TapEventArgs args)
    {
        if (args.HitObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            Debug.Log("Start Dialogue here.");

        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }
    }
}
