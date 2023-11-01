# Documentación de la evidencia evaluativa de la unidad

## Enlace al video en YouTube
https://youtu.be/7SqK5wH4FA4

## Explicación de la solución

This time, the challenge was to design and develop an ecosystem composed by (at least) three creatures. The main objective was to be able to recognize and give diferentiable traits just with their behaviour, not with their visual design. 

Also, we had to manipulate their movement only by the acceleration vector, which will control the velocity and the location of each creature.

So, I first thought about a sleepy prophet, a creature that can trascend the barriers of its world and see through its reality (which means it has the ability to locate the mouse position and move really slowly towards it, like if it was following the mouse).

Then I designed an indecisve bird, which mainly moves horizontally and, although at first it seems like it's following a clear path, it changes randomly the direction. Also, it seems like it's not really sure which way to go, flying a bit clumsyly.

My third creature is a Fly who's really high on cocaine. It moves randomly on an specific area, and sometimes it "jumps" quite far from its original position and keeps moving erratically on that new area.

First, the base of all creatures' movement was the Motion 101. This basically is updating each value per frame, acceleration updates velocity, velocity is added to location and the object then is moved to the location specified. Is a chain reaction that works just like it does on real life.

I created three scripts, each one defines the class of the respective creature and generates an array.

## Sleepy Prophet

To program the prophet's behaviour I used Motion 101, and I created a vector that holds the result of substracting the vector of the mouse position and the vector of the creatures' location.

```csharp
     Vector2 dir = prophets[i].SubtractVectors(mousePos, prophets[i].location);`
```

This way I could know the direction of the mouse regarding the creature, so I just had to update the acceleration vector with a magnitude and give it the normalized direction vector that I got before.

```csharp
prophets[i].acceleration = prophets[i].ScaleVector(dir.normalized, .5f);`
```


The Motion 101, which is updated each frame, works like this:

```csharp
// Speeds up the mover
velocity += acceleration * Time.deltaTime;

// Limit Velocity to the top speed
velocity = Vector2.ClampMagnitude(velocity, topSpeed);

// Moves the mover
location += velocity * Time.deltaTime;

// Updates the GameObject of this movement
prophetMover.transform.position = new Vector3(location.x, location.y, 0);
```


## Cocaine Fly

Now, to create the cocaine fly I generated a random walk cycle based on Motion 101 this way:

```csharp
// Random acceleration but it's not normalized
acceleration = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

// Normalize the acceletation
acceleration.Normalize();

// Now we can scale the magnitude as we wish
acceleration = ScaleVector(acceleration, Random.Range(100f, 250f));

// Speeds up the mover, Time.deltaTime is the time passed since the last frame and ties acceleration to a fixed rate instead of framerate
velocity += acceleration * Time.deltaTime;

// Limit Velocity to the top speed
velocity = Vector2.ClampMagnitude(velocity, topSpeed);

// Moves the mover
location += velocity * Time.deltaTime;

// Updates the GameObject of this movement
flyMover.transform.position = new Vector2(location.x, location.y);
```

I then added the concept of Lévy flight, which is a random walk in which the step-lengths have a stable distribution. It is commonly used to describe the way some flocks (or groups of other animals) exhaustively search on an area for food, and after that they rapidly go to another area to do the same. Here, the fly flies erratically on a small area, until a random integer between 1 and 100 turns into a number smaller than 20, if this happens, the acceleration scales humungusly and the velocity doesn't clamp to the top speed, this way:

```csharp
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
}
else
{
   // Now we can scale the magnitude as we wish
   acceleration = ScaleVector(acceleration, Random.Range(100f, 250f));

   // Speeds up the mover, Time.deltaTime is the time passed since the last frame and ties acceleration to a fixed rate instead of framerate
   velocity += acceleration * Time.deltaTime;

   // Limit Velocity to the top speed
   velocity = Vector2.ClampMagnitude(velocity, topSpeed);
   
}

   
   // Moves the mover
   location += velocity * Time.deltaTime;

   // Updates the GameObject of this movement
   flyMover.transform.position = new Vector2(location.x, location.y);
```

## Indecisive Bird

To program the indecisive birds I used the cocain flies' code as a base. The birds are also following a random walk, but they tend to move more horizontically because the random values of the acceleration vector are greater on the x axis than on the y axis, also their acceleration is way less scaled and they tend to keep their direction because of this (the new values of acceleration slightly changes their direction).

```csharp
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
```

## Mejoras para la próxima entrega

1. Identificar y describir el problema a resolver. Tiene que ver con la narrativa y las decisiones de diseño.
2. Con qué concepto matemático y/o físico se resuelve ese problema y POR QUÉ
3. La parte del cóidigo donde se ve aplicado el concepto.



