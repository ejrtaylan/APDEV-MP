using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] GameObject diceRoller;

    private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DICE_RESULT, this.OnEnd);
    }

    public void Update()
    {
        //Debug.Log("shit");
        if(Input.touchCount > 0)
        {
            Parameters param = new Parameters();
            param.PutExtra("DIFFICULTY_CLASS", 20);
            EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, param);

            diceRoller.SetActive(true);
        }

    }

    void OnEnd(Parameters param)
    {
        bool yes = param.GetBoolExtra("ROLL_RESULT", false);

        if (yes)
        {
            Debug.Log("yes");
        }
        else Debug.Log("no");
    }
}
