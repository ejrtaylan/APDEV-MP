using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResultScript : MonoBehaviour
{
    [SerializeField] DiceRollScript diceRoll;
    [SerializeField] GameObject dicePrefab;

    int difficulty; //difficulty check
    int modifier; //modifier from skills
    public bool diceSuccess; //whether or not roll succeeded

    void OnEnable()
    {
        this.difficulty = 0;
        this.modifier = 0;
        this.diceSuccess = false;

        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, this.ChangeDifficulty);
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ADD_MODIFIER, this.AddModifier);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.ADD_MODIFIER);
    }
    void AddModifier(Parameters param)
    {
        this.modifier = param.GetIntExtra("MODIFIER", 0);
    }

    void ChangeDifficulty(Parameters param)
    {
        this.difficulty = param.GetIntExtra("DIFFICULTY_CLASS", 1);
    }

    void CallDiceRollEnd()
    {
        Parameters param = new Parameters();

        int finalresult = diceRoll.result + this.modifier;

        if (finalresult < this.difficulty)
        {
            this.diceSuccess = false;

            Debug.Log("failed");

            //Continue to DiceAds script
            EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.DICE_ADS);
        }

        else if (finalresult >= this.difficulty)
        {
            this.diceSuccess = true;

            //Finish dice interaction
            param.PutExtra("ROLL_RESULT", this.diceSuccess);
            EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DICE_RESULT, param);

            dicePrefab.SetActive(false);
        }
    }

    void Update()
    {
        if(diceRoll.doneRolling)
        {
            Invoke("CallDiceRollEnd", 1.5f);
            EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DICE_DONE);
            diceRoll.doneRolling = false;
        }
    }
}
