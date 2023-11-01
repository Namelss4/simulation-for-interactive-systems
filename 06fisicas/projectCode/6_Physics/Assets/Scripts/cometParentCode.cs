using UnityEngine;

public class cometParentCode : MonoBehaviour
{
    public float radio = 5f; // Radio de la circunferencia
    public float velocidad = 30f; // Velocidad de rotación en grados por segundo
    private float angulo = 0f;

    void Update()
    {
        angulo += velocidad * Time.deltaTime;
        float radianes = angulo * Mathf.Deg2Rad;
        Vector3 posicionNueva = new Vector3(Mathf.Cos(radianes) * radio, transform.position.y, Mathf.Sin(radianes) * radio);
        transform.position = posicionNueva;
    }
}