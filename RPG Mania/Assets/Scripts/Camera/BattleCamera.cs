using UnityEngine;

public class BattleCamera : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Vector3 currentOffset;
    [Range(1, 10)]
    public float smoothFactor;
    [SerializeField] private Vector3 minValues, maxValues;

    private void Awake() 
    {
        currentOffset = offset;

        gameObject.SetActive(false);
    }

    private void OnEnable() 
    {
        tag = "MainCamera";
    }

    private void OnDisable() {
        tag = "Untagged";
    }

    private void SnapToPosition()
    {
        transform.position = target.position + currentOffset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = target.position + currentOffset;
        // Check if camera is out of bounds or not
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;

        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    public void SetDirection(float direction)
    {
        switch (direction)
        {
            case 90:
                currentOffset.x = -offset.z;
                currentOffset.z = -offset.x;
                break;
            case 180:
                currentOffset.z = -offset.z;
                break;
            case 270:
                currentOffset.x = offset.z;
                currentOffset.z = offset.x;
                break;
            default:
                currentOffset = offset;
                break;
        }

        SnapToPosition();
    }
}