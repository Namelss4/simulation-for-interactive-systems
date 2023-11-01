using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringConnection : MonoBehaviour
{
    // Get spring values from the inspector
    [SerializeField] float springConstantK = 3.5f;
    [SerializeField] float restLength = 3f;
    //[SerializeField] Transform anchorTransform;
    [SerializeField] Rigidbody bobBody;
    [SerializeField] GameObject bobBody2;

    [SerializeField] Material springMat, dragMat;
    [SerializeField] GameObject energyParticles;

    // Start is called before the first frame update
    void Start()
    {
        // Add a new spring at the start of runtime
        Spring3_11 spring = gameObject.AddComponent<Spring3_11>();
        spring.anchor = bobBody2.transform;
        spring.connectedBody = bobBody;
        spring.restLength = restLength;
        spring.springConstantK = springConstantK;
        spring.mat = springMat;

        // Add the click-drag behavior
        ClickDragBody3_11 mouseDrag = bobBody.gameObject.AddComponent<ClickDragBody3_11>();
        mouseDrag.dragMate = dragMat;
        mouseDrag.body = bobBody;
        mouseDrag.radius = 1;
        mouseDrag.partSyst = energyParticles;
    }
}

public class Spring3_11 : MonoBehaviour
{
    // Properties that need to be assigned by the inspector or other scripts
    public Transform anchor;
    public Rigidbody connectedBody;
    public float restLength = 1;
    public float springConstantK = 0.1f;
    public Material mat;

    LineRenderer lineRenderer;

    void Start()
    {
        // Instantiate LineRenderer, add a material and scale the width
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        // lineRenderer.material = new Material(Shader.Find("Diffuse"));
        lineRenderer.material = mat;
        lineRenderer.widthMultiplier = 0.5f;
    }

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
}

public class ClickDragBody3_11 : MonoBehaviour
{
    public Rigidbody body;
    public float radius;

    Renderer bodyRenderer;

    Material defaultMaterial;
    Material mouseOverMaterial;

    bool isDragging = false;

    public Material dragMate;

    public GameObject partSyst;

    void Start()
    {
        bodyRenderer = body.gameObject.GetComponent<Renderer>();
        defaultMaterial = bodyRenderer.material;

        partSyst.SetActive(false);
    }

    void Update()
    {
        DragBob();
    }

    void DragBob()
    {
        // Declare a Vector2 for the location of the mouse in world space
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Set the renderer material to default
        bodyRenderer.material = defaultMaterial;

        // If the mouse location is not within the radius of the body, return
        if (Vector2.Distance(mouseLocation, body.position) > radius)
            return;

        // If the mouse location is within the radius of the body, set material to mousover material
        bodyRenderer.material = dragMate;

        // If the mouse location is within the radius of the body and the mouse is clicked, set isDragging to true
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            partSyst.SetActive(true);
        }

        // If isDragging is true, lock the position of the body to the mouse position until the mouse button is released
        if (isDragging)
        {
            body.position = mouseLocation;
            if (Input.GetMouseButtonUp(0))
            {
                body.position = mouseLocation;
                body.velocity = Vector3.zero;
                isDragging = false;
                partSyst.SetActive(false);
            }
        }

    }
}