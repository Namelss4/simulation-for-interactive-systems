# Documentación de la evidencia evaluativa de la unidad

## Enlace al video en YouTube
https://youtu.be/YlQFp_ACAaM

## Explicación de la solución
The challenge I had to take on was generating smooth variation of heights on a map/terrain using Unity. 

I created a terrain, and use a script for the generation based on the Perlin noise, which is a type of gradient noise that produces a naturally ordered sequence of pseudo-random numbers. Unity has a built-in implementation of the Perlin noise algorithm, the function Mathf.PerlinNoise(x float, y float), so I used it to generate random heights that follows the rules of a Perlin noise so the changes on the value are smoother.

I also added some offstes on the x and y axis that are constantly increasing over time so that the terrain gives the impression that it's moving. I implemented a scale to make them noticeable and calculated the coordinates based on these ecuations:

    float xCoord = (float)x / width * scale + offsetX;
    float yCoord = (float)y / height * scale + offsetY;

    return Mathf.PerlinNoise(xCoord, yCoord);
Then I used those to generate heights and applying them on the terrain. 

Once the Perlin noise was working, I took on the other part of the task: Making the project somewhat interactive. 
I mainly did two things:

  1. If you hold the left arrow button on your keyboard, the offsets will decrease instead of increasing, so the terrain will seem like it's moving the opposite   way.
  2. If you hold the spacebar, the depth values will change randomly between 30 and 70 and the scale will do the same between 30 and 80, so it looks like it's really altered and shifty. When you stop holding the spacebar everything goes back to normal.

This two things work well together, since one part is not really affecting the other.
   
