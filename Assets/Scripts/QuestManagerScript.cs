using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManagerScript : MonoBehaviour
{
    bool mq1 = false;
    bool mq2 = false;
    bool mq3 = false;

    bool sq1 = false;
    bool sq2 = false;
    bool sq3 = false;

    // Start is called before the first frame update
    void OnEnable()
    {

        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_MQ1_COMPLETE, this.SetMQ1Complete);
        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_MQ2_COMPLETE, this.SetMQ2Complete);
        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_MQ3_COMPLETE, this.SetMQ3Complete);

        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_SQ1_COMPLETE, this.SetSQ1Complete);
        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_SQ2_COMPLETE, this.SetSQ2Complete);
        EventBroadcaster.Instance.AddObserver(EventNames.QuestProgression.ON_SQ3_COMPLETE, this.SetSQ3Complete);

    }

    // Update is called once per frame
    void Update()
    {
        if(mq1 && mq2 && mq3)
        {
            
        }
    }

    void SetMQ1Complete()
    {
        this.mq1 = true;
    }

    void SetMQ2Complete()
    {
        this.mq2 = true;
    }

    void SetMQ3Complete()
    {
        this.mq3 = true;
    }

    void SetSQ1Complete()
    {
        this.sq1 = true;
    }

    void SetSQ2Complete()
    {
        this.sq2 = true;
    }

    void SetSQ3Complete()
    {
        this.sq3 = true;
    }
}
