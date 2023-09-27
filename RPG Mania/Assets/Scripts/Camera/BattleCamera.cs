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

    public void SetDirection(float direction)
    {
        switch (direction)
        {
            case 90:
                currentOffset.x = -offset.z;
                currentOffset.z = -offset.x;
                break;
            case 180:
                currentOffset.x = offset.x;
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

    private void SnapToPosition()
    {
        transform.position = target.position + currentOffset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}