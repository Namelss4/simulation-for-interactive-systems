using UnityEngine;

public class raycastDarkStar : MonoBehaviour
{
    public float speed = 5f;
    public float speedRot = 5f;
    private Rigidbody rb;
    private Vector3 direction;
    private bool isMoving = false;

    void Start()
    {
        // Gets the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Creates a ray that starts just above the cube to prevent the raycast from hitting the cube itself
        Ray ray = new Ray(transform.position + transform.up * 1f, transform.up);

        // Casts the raycast upward
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If the raycast hits an object with the name "Beg"
            if (hit.collider.gameObject.name == "Beg")
            {
                // Starts moving towards the "Beg" object
                isMoving = true;
                // Calculates the direction towards the "Beg" object
                direction = (hit.collider.gameObject.transform.position - transform.position).normalized;
            }
        }

        // If the cube is moving
        if (isMoving)
        {
            // Adds force to the Rigidbody in the direction of the "Beg" object
            rb.AddForce(direction * speed);

            // If the cube has reached the "Beg" object
            //if (Vector3.Distance(transform.position, hit.collider.gameObject.transform.position) < 0.1f)
            //{
            //    // Stops moving
            //    isMoving = false;
            //    // Stops the Rigidbody
            //    rb.velocity = Vector3.zero;
            //}
        }
        else
        {
            // Rotates the cube on the Z axis
            transform.Rotate(0, 0, speedRot * Time.fixedDeltaTime);
        }
    }
}
