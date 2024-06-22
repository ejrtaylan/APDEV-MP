using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject DiceRoller;

    private bool menuShown = false;
    private bool waitingForRoll = false;

    // Start is called before the first frame update
    void Start()
    {
        this.MenuPanel.SetActive(false);
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DICE_RESULT, this.AwaitRoll);
    }

    public void OnToggleMenu(){
        this.menuShown = !this.menuShown;
        this.MenuPanel.SetActive(menuShown);

    }

    public void OnAutoWinRoll(){
        Debug.Log("Win it!");

        this.DiceRoller.SetActive(true);
        this.waitingForRoll = true;

        Parameters param = new Parameters();
        param.PutExtra("DIFFICULTY_CLASS", 0);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, param);
    }

    public void OnAutoLoseRoll(){
        Debug.Log("Lose it!");

        this.DiceRoller.SetActive(true);
        this.waitingForRoll = true;

        Parameters param = new Parameters();
        param.PutExtra("DIFFICULTY_CLASS", 30);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, param);
    }

    public void AwaitRoll(Parameters param){
        if(!this.waitingForRoll) return;

        this.waitingForRoll = false;
        this.DiceRoller.SetActive(false);

        Debug.Log("Result: " + param.GetIntExtra("ROLL_RESULT", -1));
    }
}
