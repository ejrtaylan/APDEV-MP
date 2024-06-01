using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GestureManager : MonoBehaviour
{	
    public static GestureManager Instance;
	private Touch _trackedFinger;
	private float _gestureTime;

    private Vector2 _startPoint = Vector2.zero;
    private Vector2 _endpoint = Vector2.zero;

    [SerializeField] private TapProperty _tapProperty;
    [SerializeField] private SwipeProperty _swipeProperty;
    public EventHandler<TapEventArgs> OnTap;
    public EventHandler<SwipeEventArgs> OnSwipe;
	
	private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void CheckTap()
    {
        if(this._gestureTime <= this._tapProperty.Time && 
           Vector2.Distance(this._startPoint, this._endpoint) <= (Screen.dpi * this._tapProperty.MaxDistance))
        {
            this.FireTapEvent();
        }
    }

    private void FireTapEvent()
    {
        GameObject hitObject = this.GetHitObject(this._startPoint);

        TapEventArgs args = new TapEventArgs(this._startPoint, hitObject);

        if(this.OnTap != null)
        {
            this.OnTap(this, args);
        }

        if(hitObject != null)
        {
            ITappable handler = hitObject.GetComponent<ITappable>();
            if(handler != null)
                handler.OnTap(args);
        }
    }

    private GameObject GetHitObject(Vector2 screenPoint)
    {
        GameObject hitObject = null;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hitObject = hit.collider.gameObject;
        }

        return hitObject;
    }

    private void CheckSwipe()
    {
        if(this._gestureTime <= this._swipeProperty.Time &&
            Vector2.Distance(this._startPoint, this._endpoint) >= (Screen.dpi * this._swipeProperty.MinDistance))
            {
                this.FireSwipeEvent();
            }
    }

    private void FireSwipeEvent()
    {
        if(this.OnSwipe != null)
        {
            GameObject hitObject = this.GetHitObject(this._startPoint);

            Vector2 rawDirection = this._endpoint - this._startPoint;
            ESwipeDirection direction = this.GetSwipeDirection(rawDirection);

            SwipeEventArgs args = new SwipeEventArgs(direction);
            this.OnSwipe(this, args);

            Debug.Log(direction);

            /*if(hitObject != null)
            {
                //ISwipeable handler = hitObject.GetComponent<ISwipeable>();
                if(handler != null)
                    handler.OnSwipe(args);
            }*/
        }
    }

    private ESwipeDirection GetSwipeDirection(Vector2 rawDirection)
    {
        if(Math.Abs(rawDirection.x) > Math.Abs(rawDirection.y))
        {
            if(rawDirection.x > 0)
                return ESwipeDirection.RIGHT;
            else
                return ESwipeDirection.LEFT;
        }

        else if(Math.Abs(rawDirection.x) < Math.Abs(rawDirection.y))
        {
            if (rawDirection.y > 0)
                return ESwipeDirection.UP;
            else
                return ESwipeDirection.DOWN;
        }
        
        return ESwipeDirection.UP;
    }

    private void Update() 
    {
        if(Input.touchCount > 0)
        {
            this._trackedFinger = Input.GetTouch(0);
            switch(this._trackedFinger.phase)
            {
                case TouchPhase.Began:
                    this._startPoint = this._trackedFinger.position;
                    this._gestureTime = 0;
                    break;
                case TouchPhase.Ended: 
                    this._endpoint = this._trackedFinger.position;
                    this.CheckTap();
                    this.CheckSwipe();
                    break;
                default:
                    this._gestureTime += Time.deltaTime;
                    break;
            }
        }
	}
}
