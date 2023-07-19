using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    [SerializeField] private Vector3 minValues, maxValues;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        // Check if camera is out of bounds or not
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.y),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
