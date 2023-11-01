using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyManager : MonoBehaviour
{
    // Create an array of 5 movers
    private CocaineFly[] flies = new CocaineFly[2];    

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate each mover in the array as a new mover
        for (int i = 0; i < flies.Length; i++)
        {
            flies[i] = new CocaineFly();
        }
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        
        for (int i = 0; i < flies.Length; i++)
        {
            flies[i].Step();
            flies[i].CheckEdges();
        }
    }
}

public class CocaineFly
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;
    private int rand = 0;
    private bool prevLevy = false;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject flyMover = GameObject.CreatePrimitive(PrimitiveType.Cube);


    public CocaineFly()
    {
        FindWindowLimits();

        // Vector2.zero is shorthand for a (0, 0) vector
        location = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)); ;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        // Set top speed to 5f
        topSpeed = 2f;

        // Set the scale to 0.4f
        flyMover.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    public void Step()
    {
        rand = Random.Range(1, 100);

        // Random acceleration but it's not normalized
        acceleration = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        // Normalize the acceletation
        acceleration.Normalize();

        if (rand < 20)
        {
            acceleration = ScaleVector(acceleration, Random.Range(30000f, 30300f));

            // Speeds up the mover, Time.deltaTime is the time passed since the last frame and ties acceleration to a fixed rate instead of framerate
            velocity += acceleration * Time.deltaTime;

            prevLevy = true;

        }
        else
        {
            if (prevLevy)
            {
                acceleration += acceleration * (-1);

                prevLevy = false;
            }
            else
            {
                // Now we can scale the magnitude as we wish
                acceleration = ScaleVector(acceleration, Random.Range(100f, 250f));

                // Speeds up the mover, Time.deltaTime is the time passed since the last frame and ties acceleration to a fixed rate instead of framerate
                velocity += acceleration * Time.deltaTime;

                // Limit Velocity to the top speed
                velocity = Vector2.ClampMagnitude(velocity, topSpeed);

                prevLevy = false;

            }
        }

        
        // Moves the mover
        location += velocity * Time.deltaTime;

        // Updates the GameObject of this movement
        flyMover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
        }
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0 for x and y. We will use -10 for z in this example
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    public Vector2 ScaleVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}