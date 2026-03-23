using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [Header("Rotación")]
    public float velocidadRotacion = 60f;

    [Header("Movimiento vertical")]
    public float altura = 0.25f;
    public float velocidadFlotacion = 2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        Rotar();
        Flotar();
    }

    void Rotar()
    {
        transform.Rotate(0f, velocidadRotacion * Time.deltaTime, 0f);
    }

    void Flotar()
    {
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * altura;

        transform.position = new Vector3(
            transform.position.x,
            nuevaY,
            transform.position.z
        );
    }
}