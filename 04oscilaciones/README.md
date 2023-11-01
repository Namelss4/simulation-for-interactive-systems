# Documentación de la evidencia evaluativa de la unidad

## Enlace al video en YouTube
https://youtu.be/lysOJ1Zv3qk

## Explicación de la solución
## The challenge
I was meant to create an interactive simulation of an ecosystem where different species interact and behave using concepts of oscillations, simple harmonic motion, pendulums, and spring forces. I already had the ecosystem working with forces (you can check that out on the 3rd unit) and its inhabitants, the planet Earth and a bunch of asteroids orbiting aggresively around it. For this challenge I wanted to make them feel less lonely.

I designed a small spacecraft that carries around a metallic object (sort of an wrecking ball) tied with a spring-like object, this spacecraft will move around controlled by the user with the arrow keys, and the user can also drag the wrecking ball to trully appreciate how the spring works (and to mess around the environmet a little). I also wanted to add something bigger... Way bigger. I ended up adding a gigantic hyperspace raging worm-like creature that kicks all other objects far away from it. This worm behaves following a simple harmonic motion, similar of that of a wave, and only appears as long as the player is holding down the J key.

## The maths and physics behind the solution
Let's split this out on the two new compannions and the changes on the previous stuff:

### 1. The spacecraft (and its little wrecking ball)
To get the spacecraft to move around I applied the force concept explained on the previous unit. Then, we'll have to discuss the spring force concept.

Spring force is the force exerted by a compressed or stretched spring upon any object that is attached to it. The force is proportional to the displacement of the spring from its equilibrium position. This relationship is known as Hooke's law:

```F = -kx```

Where F is the force in newtons (N), x is the displacement in meters (m) and k is the spring constant unique to the object in newtons/meter (N/m).

The force of the spring is directly proportional to the extension of the spring. In other words, if you pull on the bob a lot, the force will be strong; if you pull on the bob a little, the force will be weak. 

I used this concept for the interaction between the spacecraft and the wrecking ball.

### 2. The gigantic hyperspace raging worm-like creature
I love this one.

First, we need to understand that a simple harmonic motion is a type of periodic motion where the restoring force is directly proportional to the displacement from equilibrium position and acts in the opposite direction of the displacement. The motion is sinusoidal in nature and can be described using the concepts of amplitude, frequency and period.

 All of this summs up on this equation:

```x = A cos(ωt + φ)```

Where x is the displacement of the object from its equilibrium position, A is the amplitude of the motion, ω is the angular frequency of the motion, t is time and φ is the phase angle.

I used this to make something bigger: A wave-like movement. A wave is a disturbance that travels through space and time, usually accompanied by the transfer of energy. The amplitude of a wave is the maximum displacement of the wave from its equilibrium position. The period of a wave is the time it takes for one complete cycle of the wave to pass a given point.

I used this concept to make the gigantic hyperspace raging worm-like creature move wimulation a worm.

### 3. The earth and the asteroids oscillate

The Earth rotates when you move it around, and the asteroids face the Earth always, how so? Because they follow this rule:

```
angle = angle + angular velocity

angular velocity = angular velocity + angular acceleration
```

To make something rotate you just have to apply an angular acceleration (or an angular force, using torque and moment of inertia).

## The code
First, I'd like you to take a look at the shaders, the ecosystem looks pretty (finally).

To make the Earth rotate as you move it around, I applied the concept of angular acceleration. Luckily enough, Unity has a function called transform.Rotate() that applies it automatically, it takes a Vector3 argument as an Euler angle, it can rotate on the local axis or the global axis, I did't use that argument and just went with the Vector3, so it works on the local axis as well. At first I created a Vector3 named rotateVector and changed its coordinates based on the direction the player is moving and created a method to the Earth class called Rotate:

```c#

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
        
```
The Rotate method works like this:
```c#
public void Rotate(Vector3 rotVect)
    {
        body.transform.Rotate(rotVect);
    }
```

I used a function called transform.LookAt() to make the asteroids always face the earth (in a way):
```c#
foreach (Asteroid m in movers)
        {
            Rigidbody body = m.body;
            Vector2 force = a.Attract(body); // Apply the attraction from the Attractor on each Mover object

            m.transform.LookAt(target);

            m.ApplyForce(force);
            m.CalculatePosition();
        }
```

For the simulation of the gigantic hyperspace raging worm-like creature (yeah, I'll keep saying the whole name) I instantiated a bunch of spheres that form the body. Then I applied the physics this way:

```c#
        // Advance the wave relative to time
        startAngle += waveSpeed * Time.deltaTime;

        // Calculate the position of each object in the wave
        float currentAngle = startAngle;
        float currentX = -maximumPos.x;
        foreach(Transform waveTransform in waveTransforms)
        {
            waveTransform.transform.localScale = new Vector3(2f, 2f, 2f);

            // Step along the circle, a larger period means steps are smaller
            currentAngle += 1 / period;

            // Remap the sin function so that y(-1, 1) corresponds to y(bottom, top)
            float currentY =  Mathf.Lerp(-maximumPos.y, maximumPos.y, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(currentAngle)));

            // Set the position of each wave transform
            waveTransform.position = new Vector2(currentX, currentY);

            // Step along the screen width such that every waver is on screen
            currentX += (maximumPos.x - -maximumPos.x) / amountWavers;
        }
```
For the spring I used a component called LineRenderer that, well, renders a line (wow) between the anchor (the spacecraft) and the bob (the wrecking ball). I applied the concept of Spring Force this way:

```c#
 void FixedUpdate()
    {
        // Get the difference Vector3 between the anchor and the body
        Vector3 force = connectedBody.position - anchor.position;

        // Find the distance/magnitude of the vector using .magnitude
        float currentLength = force.magnitude;
        float stretchLength = currentLength - restLength;

        // Reverse the direction of the force arrow and set its length
        // based on the spring constant and stretch
        force = -force.normalized * springConstantK * stretchLength;

        // Apply the force to the connected body relative to time
        connectedBody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);

        // Draw the line along the spring
        lineRenderer.SetPosition(0, anchor.position);
        lineRenderer.SetPosition(1, connectedBody.position);
    }
```

