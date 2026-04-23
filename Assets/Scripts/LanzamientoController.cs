using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanzamientoController : MonoBehaviour
{
    public enum Estado
    {
        Esperando,
        SeleccionAngulo,
        SeleccionFuerza,
        Lanzado
    }

    public Estado state = Estado.Esperando;

    [Header("Referencias")]
    public RectTransform flechaUI;
    public Image barraFuerza;
    public GameObject poncho;
    public TMP_Text txtIntruccion;

    [Header("Angulo")]
    public float velocidadRotacion = 120f;
    private float anguloActual = 0f;
    private bool subiendo = true;

    [Header("Fuerza")]
    public float fuerzaMin = 1f;
    public float fuerzaMax = 100f;
    public float velocidadDeCarga = 100f;
    private float fuerzaActual = 0f;
    private float multiplicadorFuerza = 0.4f;
    private bool cargandoSube = true;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = poncho.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == Estado.Esperando)
        {
            txtIntruccion.text = "Presiona la pantalla para elegir el angulo del lanzamiento";
            if (Input.GetMouseButtonDown(0))
            {
                state = Estado.SeleccionAngulo;
                flechaUI.gameObject.SetActive(true);
            }
        } else if (state == Estado.SeleccionAngulo)
        {
            if (Input.GetMouseButton(0))
            {
                actualizarAngulo();
                txtIntruccion.text = "Deja de presionar para seleccionar el angulo";
            }

            if (Input.GetMouseButtonUp(0))
            {
                state = Estado.SeleccionFuerza;
                flechaUI.gameObject.SetActive(false);
                barraFuerza.gameObject.SetActive(true);
                txtIntruccion.text = "Presiona la pantalla para determinar la fuerza";
            }
        }else if (state == Estado.SeleccionFuerza)
        {
            if (Input.GetMouseButton(0))
            {
                ActualizarFuerza();
                txtIntruccion.text = "Deja de presionar para seleccionar la fuerza";
            }

            if (Input.GetMouseButtonUp(0))
            {
                barraFuerza.gameObject.SetActive(false);
                LanzarPoncho();
                state = Estado.Lanzado;
                txtIntruccion.text = string.Empty;
            }
        }
    }

    private void LanzarPoncho()
    {
        Rigidbody rb = poncho.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        float rad = anguloActual * Mathf.Deg2Rad;
        Vector3 direccion = new Vector3(0, Mathf.Sin(rad), Mathf.Cos(rad));
        Debug.Log(direccion);
        poncho.transform.rotation = Quaternion.identity;
        rb.velocity = direccion * fuerzaActual * multiplicadorFuerza;
    }

    private void ActualizarFuerza()
    {
        if (cargandoSube)
        {
            fuerzaActual += velocidadDeCarga * Time.deltaTime;
        }
        else
        {
            fuerzaActual -= velocidadDeCarga * Time.deltaTime;
        }

        if (fuerzaActual >= fuerzaMax)
        {
            fuerzaActual = fuerzaMax;
            cargandoSube = false;
        }

        if (fuerzaActual <= fuerzaMin)
        {
            fuerzaActual = fuerzaMin;
            cargandoSube = true;
        }

        barraFuerza.fillAmount = fuerzaActual / fuerzaMax;
    }

    private void actualizarAngulo()
    {
        if (subiendo)
        {
            anguloActual += velocidadRotacion * Time.deltaTime;
        }
        else
        {
            anguloActual -= velocidadRotacion * Time.deltaTime;
        }

        if (anguloActual >= 90f)
        {
            anguloActual = 90f;
            subiendo = false;
        }

        if (anguloActual <= 0f)
        {
            anguloActual = 0f;
            subiendo = true;
        }
        Debug.Log("Angulo: " + anguloActual);
        flechaUI.localRotation = Quaternion.Euler(0,0,anguloActual);
    }
}
