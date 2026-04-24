using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poncho : MonoBehaviour
{
    //Distancia recorrida
    private Vector3 posicionInicial;
    private float distanciaRecorrida;

    [Header("Monedas")]
    public LayerMask capaMoneda;
    public Vector3 tamañoDetección = new Vector3(2f,2f,2f);
    public float puntosPorMoneda = 10f;
    public float puntos = 0;

    private Rigidbody rb;
    private bool enVuelo = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (enVuelo)
        {
            calcularDistancia();
            detectarMonedas();

            GameManager.instance.ActualizarUI(distanciaRecorrida, puntos);
        }
    }

    public void iniciarVuelo()
    {
        posicionInicial = transform.position;
        distanciaRecorrida = 0f;
        puntos = 0;
        enVuelo = true;
    }

    void calcularDistancia()
    {
        distanciaRecorrida = Vector3.Distance(posicionInicial, transform.position);
    }

    void detectarMonedas()
    {
        Collider[] monedas = Physics.OverlapBox(transform.position,
                                                tamañoDetección/2,
                                                Quaternion.identity,
                                                capaMoneda);

        foreach (Collider moneda in monedas)
        {
            if (moneda.CompareTag("Moneda"))
            {
                puntos += puntosPorMoneda;
                Destroy(moneda.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enVuelo) return;
        
        if (collision.gameObject.CompareTag("Publico"))
        {
            enVuelo = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            GameManager.instance.FinDelJuego(distanciaRecorrida, puntos);
        }
    }

    public void reiniciar(Vector3 nuevaPosicion)
    {
        transform.position = posicionInicial;
        transform.rotation = Quaternion.identity;
        if (!rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        rb.isKinematic = true;

        distanciaRecorrida = 0f;
        puntos = 0;
        enVuelo = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, tamañoDetección);
    }
}
