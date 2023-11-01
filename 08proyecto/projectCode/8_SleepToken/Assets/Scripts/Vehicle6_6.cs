using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle6_6 : MonoBehaviour
{
    // Variables accessible to other scripts.
    public float maxspeed;
    public Rigidbody2D body;
    public Material lineMaterial;

    // Update is called once per frame
    void Update()
    {
        // Look in the direction the vehicle is traveling in.
        // Vector3.back must be specified since that is the "up" direction in our scene.
        gameObject.transform.LookAt(transform.position + (Vector3)body.velocity, Vector3.up);
    }

    public void Seek(Vector2 target, float scaleMultiplier, float startScale)
    {
        // Get a vector pointing from our location to the target.
        Vector2 desired = target - body.position;
        // Scale our desired vector by our maximum speed.
        desired = desired.normalized * maxspeed;

        // Apply Reynold's path following force relative to time.
        Vector2 steer = desired - body.velocity;
        //body.velocity = steer * Time.fixedDeltaTime * ((AudioManager.bandBuffer[7] * scaleMultiplier) + startScale);
        body.AddForce(steer * Time.fixedDeltaTime, ForceMode2D.Impulse);
        //body.AddForce((steer * Time.fixedDeltaTime * ((AudioManager.bandBuffer[7] * scaleMultiplier) + startScale)), ForceMode2D.Impulse);
        //body.velocity = new Vector2((AudioManager.bandBuffer[0] * scaleMultiplier) + startScale, (AudioManager.bandBuffer[0] * scaleMultiplier) + startScale);
    }

    public void FollowPath(Path6_6 path, float scaleMultiplier, float startScale)
    {
        // Predict the future location of the body.
        Vector2 predictedLocation = body.position + body.velocity.normalized * 2.5f;

        float distanceRecord = float.MaxValue;
        Vector2 recordTarget = Vector2.zero;
        // Look at each segment and find the closest normal point.
        for (int i = 0; i < path.points.Length - 1; i++)
        {
            Vector2 a = path.points[i].position;
            Vector2 b = path.points[i + 1].position;
            Vector2 normalPoint = GetNormalPoint(predictedLocation, a, b);
            // If the normal point is beyond the line segment, clamp it to the endpoint.
            if (normalPoint.x > b.x || normalPoint.x < a.x)
            {
                normalPoint = b;
            }

            // If this point is closer than any previous point, update the record.
            float distance = Vector2.Distance(predictedLocation, normalPoint);
            if (distance < distanceRecord)
            {
                distanceRecord = distance;
                recordTarget = normalPoint;
            }
        }

        // Is the vehicle predicted to leave the path?
        if (distanceRecord > path.radius)
        {
            // If so, steer the vehicle towards the path.
            Seek(recordTarget, scaleMultiplier, startScale);
        }

    }

    private Vector2 GetNormalPoint(Vector2 point, Vector2 start, Vector2 end)
    {
        // Treat start as the origin of our problem.
        Vector2 ap = point - start;
        Vector2 ab = end - start;

        // Scale the vector by the dot product to find the nearest point to p.
        ab.Normalize();
        ab *= Vector2.Dot(ap, ab);

        // Re-add the relative position of our input.
        Vector2 normalPoint = ab + start;
        return normalPoint;
    }
   
}