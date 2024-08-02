using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public Vector3 targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = targetPosition;
    }
}
