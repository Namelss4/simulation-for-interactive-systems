using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(
            Mathf.PerlinNoise(Time.time, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time) - 0.5f,
            Mathf.PerlinNoise(Time.time, Time.time) - 0.5f
        );
        rb.AddForce(direction * speed);
    }
}
