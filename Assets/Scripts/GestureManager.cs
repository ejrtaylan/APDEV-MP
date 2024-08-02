using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GestureManager : MonoBehaviour
{	
    public static GestureManager Instance;
	private Touch _trackedFinger;
	private float _gestureTime;
    GameObject DragObject = null;

    private Vector2 _startPoint = Vector2.zero;
    private Vector2 _endpoint = Vector2.zero;

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private TapProperty _tapProperty;
    [SerializeField] private SwipeProperty _swipeProperty;
    public EventHandler<TapEventArgs> OnTap;
    public EventHandler<SwipeEventArgs> OnSwipe;

    [SerializeField]
    private DragProperty _dragProperty;
    public EventHandler<DragEventArgs> OnDrag;

    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Reset()
    {
        if (this.DragObject != null)
        {
            this.DragObject.transform.position = this.DragObject.transform.position + new Vector3(1000, 1000, 1000);
            GameObject hitObject = GetHitObject(this._endpoint);
            this.DragObject.transform.position = this.DragObject.transform.position - new Vector3(1000, 1000, 1000);
            DragEventArgs args = new(this._trackedFinger, hitObject);

            IResettable target = this.DragObject.GetComponent<IResettable>();
            if (target != null)
                target.OnReset(args);
        }

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

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPoint;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            return results[0].gameObject;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, raycastMask, QueryTriggerInteraction.Ignore))
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


    private void CheckDrag()
    {
        if (this._gestureTime >= this._dragProperty.Time)
            this.FireDragEvent();
    }

    private void FireDragEvent()
    {
        Vector2 position = this._trackedFinger.position;
        GameObject hitObject = this.GetHitObject(position);
        if (hitObject != null && this.DragObject == null) this.DragObject = hitObject;
        DragEventArgs args = new DragEventArgs(this._trackedFinger, this.DragObject);

        if (this.OnDrag != null)
            this.OnDrag(this, args);

        if (this.DragObject != null)
        {
            IDraggable handler = this.DragObject.GetComponent<IDraggable>();
            if (handler != null)
                handler.OnDrag(args);
        }

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
                    this.Reset();
                    DragObject = null;
                    this.CheckTap();
                    this.CheckSwipe();
                    break;
                default:
                    this._gestureTime += Time.deltaTime;
                    this.CheckDrag();
                    break;
            }
        }
	}
}
