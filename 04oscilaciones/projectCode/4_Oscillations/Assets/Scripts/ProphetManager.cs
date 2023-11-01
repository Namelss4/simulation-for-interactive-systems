using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphetManager : MonoBehaviour
{
    // Create an array of 3 movers
    private SleepyProphet[] prophets = new SleepyProphet[3];
    //private int rand = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate each mover in the array as a new mover
        for (int i = 0; i < prophets.Length; i++)
        {
            prophets[i] = new SleepyProphet();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //rand = Random.Range(1, 100);

        for (int i = 0; i < prophets.Length; i++)
        {
            Vector2 dir = prophets[i].SubtractVectors(mousePos, prophets[i].location);

            //if(rand != 1)
            //{
                prophets[i].acceleration = prophets[i].ScaleVector(dir.normalized, .5f);
            //}
            //else
            //{
                //prophets[i].acceleration = prophets[i].ScaleVector(dir.normalized, 300.5f);
                //Debug.Log("Entra");
            //}

            prophets[i].Step();
            prophets[i].CheckEdges();
        }
    }
}

public class SleepyProphet
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject prophetMover = GameObject.CreatePrimitive(PrimitiveType.Capsule);

    public SleepyProphet()
    {
        FindWindowLimits();

        location = new Vector2(Random.Range(-6f, 6f), Random.Range(-6f, 6f));

        // Vector2.zero is shorthand for a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        // Set top speed to 0.5f
        topSpeed = 0.5f;

        // Set the scale to 0.5f
        prophetMover.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void Step()
    {
        if (velocity.magnitude <= topSpeed)
        {
            // Speeds up the mover
            velocity += acceleration * Time.deltaTime;

            // Limit Velocity to the top speed
            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            // Moves the mover
            location += velocity * Time.deltaTime;

            // Updates the GameObject of this movement
            prophetMover.transform.position = new Vector3(location.x, location.y, 0);
        }
        else
        {
            velocity -= acceleration * Time.deltaTime;
            location += velocity * Time.deltaTime;
            prophetMover.transform.position = new Vector3(location.x, location.y, 0);
        }
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
            ResetMovementVariables();
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
            ResetMovementVariables();
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
            ResetMovementVariables();
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
            ResetMovementVariables();
        }
    }

    private void ResetMovementVariables()
    {
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
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

    // This method calculates A - B component wise
    // SubtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    public Vector2 SubtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates a vector scaled by a factor component wise
    // ScaleVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    public Vector2 ScaleVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }

}
