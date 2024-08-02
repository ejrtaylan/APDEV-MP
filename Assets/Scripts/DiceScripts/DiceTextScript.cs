using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTextScript : MonoBehaviour
{
    [SerializeField] DiceRollScript diceRoll;
    [SerializeField] TMPro.TextMeshProUGUI resultText;
    [SerializeField] TMPro.TextMeshProUGUI difficultyClassText;

    void OnEnable()
    {
        this.resultText.text = "Shake to roll!";
        this.difficultyClassText.text = "";

        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, this.ChangeDifficultyClassText);
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DICE_DONE, this.ChangeResultText);
    }

    void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.ON_DICE_DONE);
    }

    void ChangeDifficultyClassText(Parameters param)
    {
        this.difficultyClassText.text = param.GetIntExtra("DIFFICULTY_CLASS", 1).ToString();
    }

    void ChangeResultText()
    {
        this.resultText.text = diceRoll.result.ToString();
    }
}
