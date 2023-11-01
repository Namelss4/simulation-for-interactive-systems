# Documentación de la evidencia evaluativa de la unidad

## Enlace al video en YouTube
https://youtu.be/b6jwHWXp58c

## Explicación de la solución
## The challenge
I was meant to add forces to an ecosystem. I had to emulate forces of attraction and repulsion between the creatures that live on the ecosystem and their world, also being interactive (using mouse and keyboard).
I chose to create a new ecosystem for this task, mainly because I really, really didn't like the one of the previous unit. I designed a galatic environment where the earth is being chased by a lot of asteroids that are pulled to it because of its gravity. I wanted to emulate an acceleration force on the earth, making it capable of moving by its own. As a last detail, I wanted to add to other forces, attraction and repulsion to an specific point on the map: the mouse location.

## The maths and physics behind the solution
As one can expect, I mainly applied the physics concept of forces. For this, I used Newton's Laws of Motion. A force is a vector that causes an object with mass to accelerate, thus replacing the previous (and quite impractical) way of dealing with acceleration, velocity and location: Motion 101. Using forces, the motor itself would be the one that calculates the accelerations applied to the objects, and with this the velocity and location. There are several forces that I could simulate, the ones that I chose were:

1. Gravity's influence: the asteroids are being pulled (or attracted) to the Earth because of its huge mass.
2. Acceleration that causes movement: I wanted the Earth to move freely, so I applied the concept of a force that impulses an object to move at will, like it is propulsing itself on a designated direction.
3. Force field: a vector field indicating the forces exerted by one object on another, when a force field is created, all objects are linked to the forces generated. I emulated this concept with the mouse interactivity, it's like you're creating a force field and you can decide whether it attracts or repels the asteroids.

I used forces because it's easily translated onto Unity, appliable to nearly every situation one could think of and it's a really easy-to-understand concept.

## The code
I created two classes, the Earth class and the asteroid class, then I created an array of 20 Asteroids, each initialized with a random location, a random velocity, a random mass, a random location:
```csharp
void Start()
    {
        int numberOfMovers = 20;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Quaternion quaternion = new Quaternion(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f));
            Asteroid m = new Asteroid(Random.Range(0.2f, 1f), randomVelocity, randomLocation, quaternion, astMat); //Each Mover is initialized randomly.;
            movers.Add(m);
        }
        a = new PlanetEarth(earthMat);
    }
```
Then I worked on the PlanetEarth class, generating its mass. Then I created an Attract function that receives a rigidbody, this method will receive the rigidbodies of each mover and then return a force on the direction of the attractor. I also added a AddForce method to apply the forces of the free movement of the Earth.
```csharp
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

        body = attractor.AddComponent<Rigidbody>();
        body.position = Vector2.zero;

        // Generate a radius
        radius = 2;

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        attractor.transform.position = body.position;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        attractor.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
        body.useGravity = false;
        // body.isKinematic = true;

        renderer.material = earthM;

        G = 9.8f;
    }

    public Vector2 Attract(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
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

}
```
Then, in the Asteroid class I assigned the random values given on the Start function, I also added here the ApplyForce method:
```csharp
public class Asteroid
{
    // The basic properties of a mover class
    public Rigidbody body;
    private Transform transform;
    private GameObject mover;

    private Vector2 maximumPos;

    public Asteroid(float randomMass, Vector2 initialVelocity, Vector2 initialPosition, Quaternion initialRotation, Material material)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(mover.GetComponent<SphereCollider>());
        transform = mover.transform;

        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;

        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = 1;
        body.position = initialPosition; // Default location
        body.rotation = initialRotation.normalized; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit

        renderer.material = material;

    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }
}
```
Here's where all the magic appears. I first get the mouse position, then applied the attraction from the Earth to each one of the movers by storing the resultant force of the Attract method, then I used ApplyForce on each asteroid and applied that resultant force. Then I used the mouse position to apply a force on the asteroids but only when a click is pressed. Left click will apply a force on the opposite direction of the mouse location, whereas right click will apply it on the same direction. Both forces are multiplied by a factor of 2.8 to magnimice the force vector, cause it is still based ont eh mouse location, so it has the direction but needs a bit of help to the magnitude.
```csharp
void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        foreach (Asteroid m in movers)
        {
            Rigidbody body = m.body;
            Vector2 force = a.Attract(body); // Apply the attraction from the Attractor on each Mover object

            m.ApplyForce(force);
            m.CalculatePosition();

            if (Input.GetMouseButton(0))
            {
                m.ApplyForce(-mousePos * 2.8f);
            }

            if (Input.GetMouseButton(1))
            {
                m.ApplyForce(mousePos * 2.8f);
            }
```
Then I applied a force on the Earth based on the buttons held on the keyboard, this way the arth will move and rotate on the direction given by the user:
```csharp

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
        
    }
```
