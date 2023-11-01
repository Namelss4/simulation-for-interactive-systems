using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeForcesManager : MonoBehaviour
{
    // Create a list to store multiple Movers
    List<Asteroid> movers = new List<Asteroid>();
    PlanetEarth a;

    [SerializeField]
    float movingForce, rotationValue;

    [SerializeField]
    Transform target;

    [SerializeField]
    Material earthMat, astMat;

    [SerializeField] 
    GameObject shootingStars;

    Vector3 rotateVector;

    // Start is called before the first frame update
    void Start()
    {
        shootingStars.SetActive(false);

        int numberOfMovers = 30;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector3 randomLocation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            Vector3 randomVelocity = new Vector3(Random.Range(0f, 3f), Random.Range(0f, 3f), Random.Range(0f, 3f));
            // Quaternion quaternion = new Quaternion(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f));
            Asteroid m = new Asteroid(Random.Range(0.2f, 0.5f), randomVelocity, randomLocation, astMat); //Each Mover is initialized randomly.;            
            movers.Add(m);
            //m.transform.SetParent(asParent.transform);
        }
        a = new PlanetEarth(earthMat);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        foreach (Asteroid m in movers)
        {
            Rigidbody body = m.body;
            Vector3 force = a.Attract(body); // Apply the attraction from the Attractor on each Mover object
            Debug.Log("Z" + force.z);

            m.transform.LookAt(target);

            m.ApplyForce(force);
            //m.CalculatePosition();

            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.F))
            {
                m.ApplyForce(-mousePos * 2.8f);
            }

            if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.F))
            {
                m.ApplyForce(mousePos * 2.8f);
            }


        }

        if (Input.GetKey(KeyCode.W))
        {
            a.ApplyForce(new Vector2(0f, movingForce));
            rotateVector.y = rotationValue;
        }

        if (Input.GetKey(KeyCode.S))
        {
            a.ApplyForce(new Vector2(0f, -movingForce));
            rotateVector.y = -rotationValue;
        }

        if (Input.GetKey(KeyCode.D))
        {
            a.ApplyForce(new Vector2(movingForce, 0f));
            rotateVector.x = rotationValue;
        }

        if (Input.GetKey(KeyCode.A))
        {
            a.ApplyForce(new Vector2(-movingForce, 0f));
            rotateVector.x = -rotationValue;
        }

        a.Rotate(rotateVector);

        if (Input.GetKey(KeyCode.G))
        {
            shootingStars.SetActive(true);
        }
        else
        {
            shootingStars.SetActive(false);
        }

    }
}


public class PlanetEarth
{
    private float radius;
    private float mass;
    private float G;

    private Vector2 location;

    private Rigidbody body;
    private GameObject attractor;

    public PlanetEarth(Material earthM)
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(attractor.GetComponent<SphereCollider>());
        Renderer renderer = attractor.GetComponent<Renderer>();

        attractor.name = "earth";

        body = attractor.AddComponent<Rigidbody>();
        body.position = Vector2.zero;

        attractor.AddComponent<SphereCollider>();

        // Generate a radius
        radius = 1.2f;

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        attractor.transform.position = body.position;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        attractor.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius * 10;
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezePositionZ;
        // body.isKinematic = true;

        renderer.material = earthM;

        G = 89.8f;
    }

    public Vector3 Attract(Rigidbody m)
    {
        Vector3 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void Rotate(Vector3 rotVect)
    {
        body.transform.Rotate(rotVect);
    }

}

public class Asteroid
{
    // The basic properties of a mover class
    public Rigidbody body;
    public Transform transform;
    private GameObject mover;

    private Vector3 maximumPos;

    public Asteroid(float randomMass, Vector3 initialVelocity, Vector3 initialPosition, Material material)
    {

        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(mover.GetComponent<SphereCollider>());
        transform = mover.transform;
        mover.name = "redAsteroid";



        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;

        // mover.AddComponent<SphereCollider>();

        Renderer renderer = mover.GetComponent<Renderer>();
        //renderer.material = new Material(Shader.Find("Diffuse"));
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = 1;
        body.position = initialPosition; // Default location
        // body.rotation = initialRotation.normalized; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit

        renderer.material = material;

        //FindWindowLimits();
    }

    public void ApplyForce(Vector3 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    //public void CalculatePosition()
    //{
    //    //CheckEdges();
    //}

    //private void CheckEdges()
    //{
    //    Vector3 velocity = body.velocity;
    //    if (transform.position.x > maximumPos.x || transform.position.x < -maximumPos.x)
    //    {
    //        velocity.x *= -1 * Time.deltaTime;
    //    }
    //    if (transform.position.y > maximumPos.y || transform.position.y < -maximumPos.y)
    //    {
    //        velocity.y *= -1 * Time.deltaTime;
    //    }
    //    if (transform.position.z > maximumPos.z || transform.position.z < -maximumPos.z)
    //    {
    //        velocity.z *= -1 * Time.deltaTime;
    //    }
    //    body.velocity = velocity;
    //}

    //private void FindWindowLimits()
    //{
    //    //Camera.main.orthographic = true;
    //    Camera.main.transform.position = new Vector3(0, 0, -10);
    //    maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    //}
}