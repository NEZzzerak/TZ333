using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] float speed = 4f;
   [SerializeField] float smoothTime = 0.1f;
   public Transform cameraTransform;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
         float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0; 
        right.Normalize();


        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        Vector3 targetVelocity = moveDirection * speed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    }
}
