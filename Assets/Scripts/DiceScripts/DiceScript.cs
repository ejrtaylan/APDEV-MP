using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    //[SerializeField] float torqueMin = 0.1f;
    //[SerializeField] float torqueMax = 2f;
    int rollForce = 0;
    [SerializeField] TextMeshProUGUI textBox;

    Vector3 originalPosition;
    private float timer = 0;

    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        
        this.originalPosition = transform.position;

        this.StartCoroutine(this.TakeInput());
    }

    private IEnumerator TakeInput()
    {
        //Wait for everything to resolve
        yield return new WaitForSeconds(2);

        //Continuously checking for input for 2 seconds
        while(this.timer < 2.0f)
        {
            this.timer += Time.deltaTime;

            this.rollForce += Input.accelerationEventCount;
        }

        Debug.Log(rollForce);

        //Check if phone was shaken
        if(rollForce > 0)
            this.RollDice();

        //Let dice roll for 3 seconds
        yield return new WaitForSeconds(3);

        this.StopRolling();
    }

    private void RollDice()
    {
        Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        Vector3 direction = targetPosition - transform.position;

        rb.AddForce(direction * (this.rollForce/2), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 5, ForceMode.Impulse);
    }

    private void StopRolling()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originalPosition;

        this.StopCoroutine(TakeInput());
    }
}
