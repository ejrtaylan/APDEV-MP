using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class JoystickHandler : MonoBehaviour, IDraggable, IResettable
{
    [SerializeField]
    private GameObject Player;
    Vector3 dir = Vector3.zero;

    [SerializeField]
    private float _speed = 10;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;

    [SerializeField]
    private int _strength = 2;

    public void OnDrag(DragEventArgs args)
    {
        if (args.HitObject == this.gameObject)
        {
            Vector2 worldPosition = this.transform.position;

            Vector2 position = args.TrackedFinger.position;
            Ray ray = Camera.main.ScreenPointToRay(position);

            if (args.HitObject.GetComponent<RectTransform>() == null)
            {
                worldPosition = ray.GetPoint(10);
            }
            else
            {
                worldPosition = position;
            }

            worldPosition = this.math(worldPosition);
            this._targetPosition = worldPosition;
            this.transform.position = worldPosition;
            Player.transform.position += dir * _strength * _speed * Time.deltaTime;
        }
    }

    private Vector3 math(Vector3 targetPos)
    {
        Vector3 difPos = targetPos - this._originalPosition;
        float magnitude = Mathf.Sqrt(difPos.x * difPos.x + difPos.y * difPos.y + difPos.z * difPos.z);
        dir = Vector3.zero;
        dir.x = difPos.x / magnitude;
        dir.y = difPos.y / magnitude;
        dir.z = difPos.z / magnitude;

        if (magnitude > 150)
        {
            return this._originalPosition + dir * 150;
        }
        return targetPos;
    }

    private void Awake()
    {
        this._targetPosition = this.transform.position;
        this._originalPosition = this.transform.position;
    }

    public void OnReset(DragEventArgs args)
    {
        this.transform.position = this._originalPosition;
        this._targetPosition = this._originalPosition;
    }
}
