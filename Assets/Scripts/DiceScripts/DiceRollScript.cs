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
    [SerializeField] float torqueAmount = 5f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] AudioClip rollClip;

    DiceSides diceSides;
    AudioSource audioSource;
    Rigidbody rb; //rigidbody of dice

    public int result; //dice result
    
    bool shaken; //if phone was shaken
    public bool hasRolled; //kinda redundant but eh
    public bool doneRolling; //EXTREMELY REDUNDANT

    private float timer; //for dice roll timings

    Vector3 originPosition; //Original position of the die
    Vector3 currentVelocity; //Used in calculation

    bool finalize;

    //Following is used in detecting shakes
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 1.0f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;

    
    void OnEnable()
    {
        //Dice
        this.diceSides = GetComponent<DiceSides>();
        this.rb = GetComponent<Rigidbody>();
        this.result = -1;

        //Audio
        this.audioSource = GetComponent<AudioSource>();
        this.audioSource.clip = rollClip;

        //Misc
        this.timer = 0;
        this.result = 0;
        
        this.originPosition = transform.position;
        this.finalize = false;
        this.shaken = false;
        this.hasRolled = false;
        this.doneRolling = false;

        //Shake detection
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    void Update()
    {
        if (this.timer == 0 && !this.hasRolled)
        {
            this.timer += 0.0001f;
            this.StartCoroutine(this.TakeInput());
        }

        if (this.finalize)
        {
            this.MoveDiceToCenter();
        }
        else if (!this.hasRolled)
        {
            this.timer = 0;
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

            Vector3 acceleration = Input.acceleration;
            lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            Vector3 deltaAcceleration = acceleration - lowPassValue;

            if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            {
                this.shaken = true;
            }
        }

        //Check if phone was shaken
        if (this.shaken)
        {
            this.shaken = false;
            this.hasRolled = true;
            this.PerformInitialRoll();

            //Let dice roll for 5 seconds
            yield return new WaitForSeconds(4);

            this.finalize = true;
        }
    }

    void PerformInitialRoll()
    {
        this.ResetDiceState();

        Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 direction = targetPosition - transform.position;

        this.rb.AddForce(targetPosition * 10.0f, ForceMode.Impulse);
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
        this.finalize = false;
        this.ResetDiceState();

        this.audioSource.Stop();

        this.result = this.diceSides.GetMatch();
        Debug.Log($"Dice landed on {result}");

        this.doneRolling = true;
    }

    void ResetDiceState()
    {
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
        transform.position = this.originPosition;
    }
}