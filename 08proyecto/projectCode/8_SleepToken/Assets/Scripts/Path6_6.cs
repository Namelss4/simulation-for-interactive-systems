using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path6_6 : MonoBehaviour
{
    [SerializeField] public Transform[] points;
    [SerializeField] public float radius;
    [SerializeField] Material pathMaterial;

    private LineRenderer pathRenderer;

    private bool isShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Create a line renderer to draw the path
        pathRenderer = new GameObject().AddComponent<LineRenderer>();
        pathRenderer.gameObject.name = "Line";

        pathRenderer.generateLightingData = true;
        pathRenderer.material = pathMaterial;
        pathRenderer.widthMultiplier = radius * 2;
        pathRenderer.startColor = new Color(255, 255, 255, 2);
        pathRenderer.endColor = new Color(255, 255, 255, 2);
        pathRenderer.startWidth = 0f;
        pathRenderer.endWidth = 0f;

        // Get the path positions from the transforms.
        pathRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            pathRenderer.SetPosition(i, points[i].position);
        }
    }

    private void Update()
    {
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pathRenderer.startWidth = 0f;
                pathRenderer.endWidth = 0f;

                isShowing = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pathRenderer.startWidth = 0.2f;
                pathRenderer.endWidth = 0.2f;

                isShowing = true;
            }
        }
        
    }
}
