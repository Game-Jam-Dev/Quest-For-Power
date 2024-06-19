using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    [SerializeField] private Vector3 minValues, maxValues;
    private bool cameraFollow = false;

    private void OnEnable() 
    {
        //SnapToPosition();
        tag = "MainCamera";
    }

    private void OnDisable() {
        tag = "Untagged";
    }

    private void SnapToPosition()
    {
        //transform.position = target.position + offset;
        //transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Throne Room")
        {
            cameraFollow = true;
        }
        else
        {
            cameraFollow = false;
        }
    }

    private void FixedUpdate()
    {
        if ( cameraFollow )
        {
            Follow();
        }
    }

    private void Follow()
    {
        Vector3 targetPosition = target.position;
        // Check if camera is out of bounds or not
        //Vector3 boundPosition = new Vector3(
        //    Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
        //    Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
        //    Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));
        //Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    public void ToggleCameraFollow()
    { 
        cameraFollow = !cameraFollow; 
    }

    public void SetCameraFollow( bool externalCameraFollow )
    {
        cameraFollow = externalCameraFollow;
    }
}
