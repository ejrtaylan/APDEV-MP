using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeReceiver : MonoBehaviour
{
    [SerializeField] private GameObject template;
    [SerializeField] private float rollForce = 10;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnSwipe += this.OnSwipe;
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnSwipe -= this.OnSwipe;
    }

    public void OnSwipe(object sender, SwipeEventArgs args)
    {
        Debug.Log("Swipe Receiver OnSwipe");

        Collider collider = template.GetComponent<Collider>();
        Vector3 direction = new() { };

        switch(args.Direction)
        {
            case ESwipeDirection.UP:
                direction = Vector3.forward; break;
            case ESwipeDirection.DOWN:
                direction = Vector3.back; break;
            case ESwipeDirection.LEFT:
                direction = Vector3.left; break;
            case ESwipeDirection.RIGHT:
                direction = Vector3.right; break;
        }

        collider.attachedRigidbody.AddForce(direction * rollForce, ForceMode.Impulse);

        /*if(args.HitObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            Vector3 spawnPos = ray.GetPoint(5);

            GameObject.Instantiate(this.template, spawnPos, Quaternion.identity);
        }*/
    }
}
