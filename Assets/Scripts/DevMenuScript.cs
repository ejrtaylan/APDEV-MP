using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;

    private bool menuShown = false;
    private bool waitingForRoll = false;
    bool toggle = false;

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

    public void WinToggle()
    {
        if (toggle)
        {
            this.OnAutoWinRoll();
        }
        else if (!toggle)
        {
            toggle = !toggle;
            this.WinToggle();
        }
    }

    public void LoseToggle()
    {
        if (toggle)
        {
            toggle = !toggle;
            this.LoseToggle();
        }
        else if(!toggle)
        {
            this.OnAutoLoseRoll();
        }
    }

    public void OnAutoWinRoll(){
        Debug.Log("Win it!");

        this.waitingForRoll = true;

        Parameters param = new Parameters();
        param.PutExtra("MODIFIER", 20);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ADD_MODIFIER, param);
    }

    public void OnAutoLoseRoll(){
        Debug.Log("Lose it!");

        this.waitingForRoll = true;

        Parameters param = new Parameters();
        param.PutExtra("MODIFIER", -20);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ADD_MODIFIER, param);
    }

    public void AwaitRoll(Parameters param){
        if(!this.waitingForRoll) return;

        this.waitingForRoll = false;

        Debug.Log("Result: " + param.GetBoolExtra("ROLL_RESULT", false));
    }
}
