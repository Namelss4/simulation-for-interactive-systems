using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] float floorY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] Transform moverSpawnTransform;

    // Create an array of 5 movers
    private IndecisiveBird[] birds = new IndecisiveBird[3];

    private Vector2 wind = new Vector3(0f, 0.4f);

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate each mover in the array as a new mover
        for (int i = 0; i < birds.Length; i++)
        {
            birds[i] = new IndecisiveBird(moverSpawnTransform.position, leftWallX, rightWallX, floorY);
        }
    }

    // Update is called once per frame forever and ever (until you quit).
    void FixedUpdate()
    {

        for (int i = 0; i < birds.Length; i++)
        {
            Debug.Log("Hola " + birds[i].location);


            if (Input.GetKeyDown(KeyCode.Space))
            {

                birds[i].body.AddForce(wind, ForceMode.Impulse);
            }
            //birds[i].Step();
            birds[i].CheckEdges();
        }
    }
}

public class IndecisiveBird
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    public Rigidbody body;

    private float radius;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject birdMover; 

    // private Rigidbody body = GameObject.
    private float xMin;
    private float xMax;
    private float yMin;

    public IndecisiveBird(Vector3 position, float xMin, float xMax, float yMin)
    {
        FindWindowLimits();
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;

        birdMover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = birdMover.AddComponent<Rigidbody>();

        birdMover.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(birdMover.GetComponent<SphereCollider>());

        radius = 1f;

        birdMover.transform.position = position + Vector3.up * radius;

        // location = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)); ;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        // Set top speed to 5f
        topSpeed = 4f;

        // Set the scale to 0.4f
        //birdMover.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        birdMover.transform.position = position + Vector3.up * radius;

        body.mass = (4f / 3f) * Mathf.PI * (radius * radius * radius);
        //body.useGravity = false; 

        //Rigidbody body;

    }

    public void Step()
    {
        // Random acceleration but it's not normalized
        acceleration = new Vector2(Random.Range(-10f, 10f), Random.Range(-0.3f, 0.3f));

        // Normalize the acceletation
        acceleration.Normalize();

        // Now we can scale the magnitude as we wish
        acceleration *= Random.Range(60f, 70f);

        // Speeds up the mover, Time.deltaTime is the time passed since the last frame and ties acceleration to a fixed rate instead of framerate
        velocity += acceleration * Time.deltaTime;

        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;

        // Updates the GameObject of this movement
        birdMover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            // Using the absolute value here is an important safe
            // guard for the scenario that it takes multiple ticks
            // of FixedUpdate for the mover to return to its boundaries.
            // The intuitive solution of flipping the velocity may result
            // in the mover not returning to the boundaries and flipping
            // direction on every tick.

            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMin, body.position.z) + Vector3.up * radius;
        }
        if (body.position.x - radius < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
            body.position = new Vector3(xMin, body.position.y, body.position.z) + Vector3.right * radius;
        }
        else if (body.position.x + radius > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
            body.position = new Vector3(xMax, body.position.y, body.position.z) + Vector3.left * radius;
        }
        body.velocity = restrainedVelocity;
    }

    //public void CheckEdges()
    //{
    //    if (location.x > maximumPos.x)
    //    {
    //        location.x = -maximumPos.x;
    //    }
    //    else if (location.x < -maximumPos.x)
    //    {
    //        location.x = maximumPos.x;
    //    }
    //    if (location.y > maximumPos.y)
    //    {
    //        location.y = -maximumPos.y;
    //    }
    //    else if (location.y < -maximumPos.y)
    //    {
    //        location.y = maximumPos.y;
    //    }
    //}

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0 for x and y. We will use -10 for z in this example
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}