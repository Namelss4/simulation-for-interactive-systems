using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    GameObject[] sampleCube = new GameObject[512];
    public float maxScale;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject instSampleCube = (GameObject)Instantiate(sampleCubePrefab);
            instSampleCube.transform.position = this.transform.position;
            instSampleCube.transform.parent = this.transform;
            instSampleCube.name = "sampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instSampleCube.transform.position = Vector3.forward * 100;
            sampleCube[i] = instSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (sampleCube != null)
            {
                sampleCube[i].transform.localScale = new Vector3(10, (AudioManager.samples[i] * maxScale) + 2, 10);
            }
        }
    }
}
