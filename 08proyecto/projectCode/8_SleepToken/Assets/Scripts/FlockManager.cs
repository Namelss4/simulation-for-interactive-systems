using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 2, maxForce = 2;
    [SerializeField] float separationScale;
    [SerializeField] float alignmentScale;
    [SerializeField] float cohesionScale;
    [SerializeField] GameObject newParent;
    [SerializeField] Material boidMaterial, boidMaterial2, boidMaterial3, boidMaterial4, boidMaterial5, boidMaterial6;
    //[SerializeField] List <Material> matList;
    // public float sinV, sinMultSepar, sinMultCohe, sinMultAli;
    public int band, band2, band3, band4, band5, band6;
    public float scaleMultiplier, startScale;

    //float time = 0; // initialize time
    //public float frequency = 1; // set frequency of the sine wave
    //public float amplitude = 0.5f; // set amplitude of the sine wave

    [SerializeField] Mesh coneMesh; // If you want to use your own cone mesh, drop it into the editor here.

    private List<Boid> boids; // Declare a List of Vehicle objects.
    private Vector2 maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();
        boids = new List<Boid>(); // Initilize and fill the List with a bunch of Vehicles
        for (int i = 0; i < 20; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial, 1));

        }

        for (int i = 0; i < 22; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial2, 2));

        }

        for (int i = 0; i < 24; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial3, 3));

        }

        for (int i = 0; i < 26; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial4, 4));

        }

        for (int i = 0; i < 30; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial5, 5));

        }

        for (int i = 0; i < 40; i++)
        {
            float ranX = Random.Range(-1.0f, 1.0f);
            float ranY = Random.Range(-1.0f, 1.0f);
            boids.Add(new Boid(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial6, 6));

        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        foreach (Boid v in boids)
        {
            v.Flock(boids);
            /*
            v.separationScale = 1;
            v.cohesionScale = 1;
            v.alignmentScale = 1;
            */
            switch (v.boidType)
            {
                case 1:
                    v.maxForce = (AudioManager.bandBuffer[band] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band] * scaleMultiplier) + startScale;
                    break;
                case 2:
                    v.maxForce = (AudioManager.bandBuffer[band2] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band2] * scaleMultiplier) + startScale;
                    break;
                case 3:
                    v.maxForce = (AudioManager.bandBuffer[band3] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band3] * scaleMultiplier) + startScale;
                    break;
                case 4:
                    v.maxForce = (AudioManager.bandBuffer[band4] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band4] * scaleMultiplier) + startScale;
                    break;
                case 5:
                    v.maxForce = (AudioManager.bandBuffer[band5] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band5] * scaleMultiplier) + startScale;
                    break;
                case 6:
                    v.maxForce = (AudioManager.bandBuffer[band6] * scaleMultiplier) + startScale;
                    v.maxSpeed = (AudioManager.bandBuffer[band6] * scaleMultiplier) + startScale;
                    break;
                default:
                    break;
            }

        }

        if (Input.GetMouseButton(0))
        {
            boids.Add(new Boid(mousePos, -maximumPos, maximumPos, maxSpeed, maxForce, coneMesh, separationScale, cohesionScale, alignmentScale, newParent, boidMaterial, 1));          
        }

        //sinV = (amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * time) + 1));

        // increment time to animate the sine wave
        //time += Time.fixedDeltaTime;

        //sphere.transform.position = new Vector3(-6.85f,sinV,0);
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

class Boid
{
    // To make it easier on ourselves, we use Get and Set as quick ways to get the location of the vehicle
    public Vector2 location
    {
        get { return myVehicle.transform.position; }
        set { myVehicle.transform.position = value; }
    }
    public Vector2 velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    public float maxSpeed, maxForce;
    public float separationScale, cohesionScale, alignmentScale;
    public int boidType;
    private Vector2 minPos, maxPos;
    private GameObject myVehicle;
    private Rigidbody rb;

    public Boid(Vector2 initPos, Vector2 _minPos, Vector2 _maxPos, float _maxSpeed, float _maxForce, Mesh coneMesh, float _separationScale, float _cohesionScale, float _alignmentScale, GameObject newParent, Material fishMat, int _boidType)
    {
        minPos = _minPos;
        maxPos = _maxPos;
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;
        separationScale = _separationScale;
        cohesionScale = _cohesionScale;
        alignmentScale = _alignmentScale;
        boidType = _boidType;

        myVehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer renderer = myVehicle.GetComponent<Renderer>();
        renderer.material = fishMat;
        renderer.material.color = Color.blue;
        Object.Destroy(myVehicle.GetComponent<BoxCollider>());

        myVehicle.transform.position = new Vector2(initPos.x, initPos.y);

        myVehicle.AddComponent<Rigidbody>();
        rb = myVehicle.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.useGravity = false; // Remember to ignore gravity!
        //rb.constraints = RigidbodyConstraints.FreezeRotationX;
        //rb.constraints = RigidbodyConstraints.FreezeRotationY;

        myVehicle.transform.SetParent(newParent.transform);

        myVehicle.name = "Fish";

        float randScale = 0;

        switch (boidType)
        {
            case 6:
                randScale = Random.Range(0.2f, 0.8f);
                break;
            case 5:
                randScale = Random.Range(0.4f, 1f);
                break;
            case 4:
                randScale = Random.Range(0.6f, 1.2f);
                break;
            case 3:
                randScale = Random.Range(0.8f, 1.4f);
                break;
            case 2:
                randScale = Random.Range(1f, 1.6f);
                break;
            case 1:
                randScale = Random.Range(1.2f, 1.8f);
                break;
            default:
                randScale = Random.Range(1.4f, 2f);
                break;
        }
       

        myVehicle.transform.localScale = new Vector3(randScale, randScale, randScale);


        /* We want to double check if a custom mesh is
         * being used. If not, we will scale a cube up
         * instead ans use that for our boids. */
        if (coneMesh != null)
        {
            MeshFilter filter = myVehicle.GetComponent<MeshFilter>();
            filter.mesh = coneMesh;
        }
        else
        {
            myVehicle.transform.localScale = new Vector3(1f, 2f, 1f);
        }
    }

    private void CheckEdges()
    {
        if (location.x > maxPos.x)
        {
            location = new Vector2(minPos.x, location.y);
        }
        else if (location.x < minPos.x)
        {
            location = new Vector2(maxPos.x, location.y);
        }
        if (location.y > maxPos.y)
        {
            location = new Vector2(location.x, minPos.y);
        }
        else if (location.y < minPos.y)
        {
            location = new Vector2(location.x, maxPos.y);
        }
    }

    private void LookForward()
    {
        /* We want our boids to face the same direction
         * that they're going. To do that, we take our location
         * and velocity to see where we're heading. */
        Vector2 futureLocation = location + velocity;
        myVehicle.transform.LookAt(futureLocation); // We can use the built in 'LookAt' function to automatically face us the right direction

        /* In the case our model is facing the wrong direction,
         * we can adjust it using Eular Angles. */
        Vector3 euler = myVehicle.transform.rotation.eulerAngles;
        myVehicle.transform.rotation = Quaternion.Euler(euler.x + 360, euler.y + 0, euler.z + 30); // Adjust these numbers to make the boids face different directions!
    }

    public void Flock(List<Boid> boids)
    {
        Vector2 sep = Separate(boids); // The three flocking rules
        Vector2 ali = Align(boids);
        Vector2 coh = Cohesion(boids);

        sep *= separationScale; // Arbitrary weights for these forces (Try different ones!)
        ali *= alignmentScale;
        coh *= cohesionScale;

        ApplyForce(sep); // Applying all the forces
        ApplyForce(ali);
        ApplyForce(coh);

        CheckEdges(); // To loop the world to the other side of the screen.
        LookForward(); // Make the boids face forward.
    }

    public Vector2 Align(List<Boid> boids)
    {
        float neighborDist = 6f; // This is an arbitrary value and could vary from boid to boid.

        /* Add up all the velocities and divide by the total to
         * calculate the average velocity. */
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (Boid other in boids)
        {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighborDist))
            {
                sum += other.velocity;
                count++; // For an average, we need to keep track of how many boids are within the distance.
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum = sum.normalized * maxSpeed; // We desire to go in that direction at maximum speed.

            Vector2 steer = sum - velocity; // Reynolds's steering force formula.
            steer = Vector2.ClampMagnitude(steer, maxForce);
            return steer;
        }
        else
        {
            return Vector2.zero; // If we don't find any close boids, the steering force is Zero.
        }
    }

    public Vector2 Cohesion(List<Boid> boids)
    {
        float neighborDist = 6f;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (Boid other in boids)
        {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighborDist))
            {
                sum += other.location; // Adding up all the other's locations
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            /* Here we make use of the Seek() function we wrote in
             * Example 6.8. The target we seek is the average
             * location of our neighbors. */
            return Seek(sum);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public Vector2 Seek(Vector2 target)
    {
        Vector2 desired = target - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector2 steer = desired - velocity;
        steer = Vector2.ClampMagnitude(steer, maxForce);

        return steer;
    }

    public Vector2 Separate(List<Boid> boids)
    {
        Vector2 sum = Vector2.zero;
        int count = 0;

        float desiredSeperation = myVehicle.transform.localScale.x * 2;

        foreach (Boid other in boids)
        {
            float d = Vector2.Distance(other.location, location);

            if ((d > 0) && (d < desiredSeperation))
            {
                Vector2 diff = location - other.location;
                diff.Normalize();

                diff /= d;

                sum += diff;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum *= maxSpeed;

            Vector2 steer = sum - velocity;
            steer = Vector2.ClampMagnitude(steer, maxForce);


            return steer;
        }
        return Vector2.zero;
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}