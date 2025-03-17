using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
  public Transform target;
    [SerializeField] float distance = 10f;
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float collisionOffset = 0.5f;
    [SerializeField] float minVerticalAngle = -45f;
    [SerializeField] float maxVerticalAngle = 45f;
    [SerializeField] float smoothTime = 0.2f;

    private Vector3 currentVelocity;
    private float currentRotationAngle;

    void LateUpdate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        currentRotationAngle += horizontalRotation;

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        float targetVerticalAngle = currentRotationAngle;
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(targetVerticalAngle, currentRotationAngle, 0);
        Vector3 targetPosition = target.position - rotation * Vector3.forward * distance;

        RaycastHit hit;
        if (Physics.Linecast(target.position, targetPosition, out hit))
        {
            targetPosition = hit.point + hit.normal * collisionOffset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.LookAt(target);
    }
}
