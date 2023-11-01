using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroids : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "redAsteroid")
        {
            Debug.Log("Dead"); // This won't work untill I fix the asteroid movement so it orbitastes on the 3 axis
        }
    }
}
