using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] float speed; // Movement speed
    private Rigidbody rb;
    public float rotationSpeed = 60000.0f; // rotation speed in degrees per second

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(Vector2.down * speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.left * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.right * speed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, rotationSpeed * Time.fixedDeltaTime));

            // Apply the rotation to the Rigidbody
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}

