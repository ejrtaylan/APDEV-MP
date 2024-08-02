using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAds : MonoBehaviour
{
    [SerializeField] DiceResultScript diceResult;
    [SerializeField] DiceRollScript diceRoll;
    [SerializeField] GameObject dicePrefab;

    [SerializeField] GameObject adMainPanel;
    [SerializeField] TMPro.TextMeshProUGUI adMainText;
    [SerializeField] GameObject adSubPanel;
    [SerializeField] TMPro.TextMeshProUGUI adSubText;

    public string[] ads;
    int count;

    void Start()
    {
        this.adMainPanel.SetActive(false);
        this.adSubPanel.SetActive(false);
        this.adMainText.text = string.Empty;
        this.adSubText.text = string.Empty;

        this.count = 0;
    }

    void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.DICE_ADS, this.ShowPanel);
    }

    void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.DICE_ADS);
    }

    void ShowPanel()
    {
        Debug.Log("panel");

        this.adMainPanel.SetActive(true);

        if(count > 0)
        {
            this.adMainText.text = "Sorry!\\r\\n\\r\\nYou've already watched an ad.";
            this.OnDontWatchAd();
        }
        else
        {
            this.adMainText.text = "You failed the roll!\\r\\n\\r\\nWould you like to watch an ad to reroll?";
        }
    }

    public void OnWatchAd()
    {
        this.adMainPanel.SetActive(false);
        this.adMainText.text = string.Empty;
        this.adSubPanel.SetActive(true);
        this.adSubText.text = ads[Random.Range(0, 1)];

        this.count++;
    }

    public void OnDontWatchAd()
    {
        this.adMainPanel.SetActive(false);

        bool success = false;

        Parameters param = new Parameters();
        param.PutExtra("ROLL_RESULT", success);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DICE_RESULT, param);

        this.count = 0;

        Debug.Log("test");
        dicePrefab.SetActive(false);
    }

    public void OnAdEnd()
    {
        this.adSubText.text = string.Empty;
        this.adSubPanel.SetActive(false);

        diceRoll.hasRolled = false;
    }


}
