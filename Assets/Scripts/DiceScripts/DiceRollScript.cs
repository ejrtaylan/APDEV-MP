using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DiceSides))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DiceRollScript : MonoBehaviour
{
    [SerializeField] float rollForce = 50f;
    [SerializeField] float torqueAmount = 5f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] TMPro.TextMeshProUGUI resultText;
    [SerializeField] TMPro.TextMeshProUGUI difficultyClassText;

    [SerializeField] AudioClip rollClip;

    int eventCount = 0;

    int result = -1;

    DiceSides diceSides;
    AudioSource audioSource;
    Rigidbody rb;

    private float timer = 0;

    Vector3 originPosition;
    Vector3 currentVelocity;
    bool finalize = false;

    void OnEnable()
    {
        this.diceSides = GetComponent<DiceSides>();
        this.rb = GetComponent<Rigidbody>();

        this.result = -1;
        this.timer = 0;
        this.eventCount = 0;

        this.audioSource = GetComponent<AudioSource>();
        this.audioSource.clip = rollClip;

        this.resultText.text = "Shake to roll!";
        this.difficultyClassText.text = "";
        this.originPosition = transform.position;

        EventBroadcaster.Instance.AddObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE, this.ChangeDifficultyClassText);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.DiceEvents.ON_DIFFICULTY_CLASS_CHANGE);
    }

    void Update()
    {
        if (this.timer == 0)
        {
            this.timer += 0.0001f;
            this.StartCoroutine(this.TakeInput());
        }
        
        if (this.finalize)
        {
            this.MoveDiceToCenter();
        }
    }

    private IEnumerator TakeInput()
    {
        //Wait for everything to resolve
        yield return new WaitForSeconds(2);

        //Continuously checking for input for 2 seconds
        while (this.timer < 1.0f)
        {
            this.timer += Time.deltaTime;

            this.eventCount += Input.accelerationEventCount;
        }

        Debug.Log("EventNum: " + this.eventCount);

        //Check if phone was shaken
        if (this.eventCount > 0)
            this.PerformInitialRoll();

        //Let dice roll for 3 seconds
        yield return new WaitForSeconds(4);

        this.finalize = true;
    }

    void PerformInitialRoll()
    {
        this.ResetDiceState();
        this.resultText.text = "";

        Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 direction = targetPosition - transform.position;

        this.rb.AddForce(targetPosition * rollForce, ForceMode.Impulse);
        this.rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);

        this.audioSource.Play();
    }

    void MoveDiceToCenter()
    {
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, smoothTime, maxSpeed);

        if ((this.originPosition - transform.position).sqrMagnitude <= 0.1f)
        {
            this.FinalizeRoll();
        }
    }

    void FinalizeRoll()
    {
        this.audioSource.Stop();
        this.finalize = false;
        this.ResetDiceState();

        this.audioSource.Stop();

        this.result = this.diceSides.GetMatch();
        Debug.Log($"Dice landed on {result}");
        this.resultText.text = result.ToString();

        Invoke("CallDiceRollEnd", 1.5f);

    }

    void CallDiceRollEnd(){
        Parameters param = new Parameters();
        param.PutExtra("ROLL_RESULT", this.result);
        EventBroadcaster.Instance.PostEvent(EventNames.DiceEvents.ON_DICE_RESULT, param);
    }

    void ResetDiceState()
    {
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
        transform.position = this.originPosition;
    }

    void ChangeDifficultyClassText(Parameters param)
    {
        int difficulty = param.GetIntExtra("DIFFICULTY_CLASS", 1);

        this.difficultyClassText.text = difficulty.ToString();
    }
}