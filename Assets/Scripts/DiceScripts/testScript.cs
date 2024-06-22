using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using Utilities;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DiceSides))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(AudioSource))]
public class testScript : MonoBehaviour
{
    [SerializeField] float rollForce = 50f;
    [SerializeField] float torqueAmount = 5f;
    //[SerializeField] float maxRollTime = 3f;
    //[SerializeField] float minAngularVelocity = 0.1f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] TMPro.TextMeshProUGUI resultText;

    [SerializeField] AudioClip rollClip;

    int eventCount = 0;

    DiceSides diceSides;
    //AudioSource audioSource;
    Rigidbody rb;

    private float timer = 0;

    Vector3 originPosition;
    Vector3 currentVelocity;
    bool finalize = false;

    void Awake()
    {
        diceSides = GetComponent<DiceSides>();
        //audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        resultText.text = "Shake to roll!";
        originPosition = transform.position;
    }

    void Update()
    {
        if (this.timer == 0)
        {
            this.timer += 0.0001f;
            this.StartCoroutine(this.TakeInput());
        }
        
        if (finalize)
        {
            MoveDiceToCenter();
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

        Debug.Log(this.eventCount);

        //Check if phone was shaken
        if (this.eventCount > 0)
            this.PerformInitialRoll();

        //Let dice roll for 3 seconds
        yield return new WaitForSeconds(4);

        this.finalize = true;
    }

    void PerformInitialRoll()
    {
        ResetDiceState();
        resultText.text = "";

        Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 direction = targetPosition - transform.position;

        rb.AddForce(targetPosition * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);

        /*audioSource.clip = shakeClip;
        audioSource.loop = true;
        audioSource.Play();*/
    }

    void MoveDiceToCenter()
    {
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, smoothTime, maxSpeed);

        if ((originPosition - transform.position).sqrMagnitude <= 0.1f)
        {
            FinalizeRoll();
        }
    }

    void FinalizeRoll()
    {
        finalize = false;
        ResetDiceState();

        /*audioSource.loop = false;
        audioSource.Stop();
        audioSource.PlayOneShot(finalResultClip);*/

        int result = diceSides.GetMatch();
        Debug.Log($"Dice landed on {result}");
        resultText.text = result.ToString();
    }

    void ResetDiceState()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originPosition;
    }
}