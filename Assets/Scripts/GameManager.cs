using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private LanzamientoController controller;

    [Header("Monedas")]
    public List<GameObject> prefabMonedas;
    public Transform zonaSpawnMonedas;
    public Vector3 dimensioneasDeSpawn = new Vector3(5f,10f,8f);
    public int cantidadSpawns = 7;
    public List<GameObject> monedasInstanciadas = new List<GameObject>();

    [Header("Referencia")]
    public Poncho _poncho;
    public Transform posicionInicialPoncho;
    public TMP_Text txtDistancia;
    public TMP_Text txtPuntos;

    [Header("UI Fin del Juego")]
    public GameObject panelFinJuego;
    public TMP_Text txtDistanciaFinal;
    public TMP_Text txtPuntosFinal;
    public TMP_Text txtTotal;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        panelFinJuego.SetActive(false);
        posicionInicialPoncho = _poncho.gameObject.transform;
        controller = GetComponent<LanzamientoController>();
        InstaciarMonedas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstaciarMonedas()
    {
        for (int i = 0; i < cantidadSpawns; i++)
        {
            int index = UnityEngine.Random.Range(0,prefabMonedas.Count);
            Vector3 posicion = zonaSpawnMonedas.position + new Vector3(
            UnityEngine.Random.Range(-dimensioneasDeSpawn.x / 2, dimensioneasDeSpawn.x / 2),
            UnityEngine.Random.Range(-dimensioneasDeSpawn.y / 2, dimensioneasDeSpawn.y / 2),
            UnityEngine.Random.Range(-dimensioneasDeSpawn.z / 2, dimensioneasDeSpawn.z / 2)
        );
            GameObject moneda = Instantiate(prefabMonedas[index],
                                            posicion,Quaternion.identity);
            monedasInstanciadas.Add(moneda);
        }
    }

    private void OnDrawGizmos()
    {
        if (zonaSpawnMonedas == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(zonaSpawnMonedas.position, dimensioneasDeSpawn);
    }

    void limpiarMonedas()
    {
        foreach (GameObject moneda in monedasInstanciadas)
        {
            if (moneda != null)
            {
                Destroy(moneda);
            }
        }
        monedasInstanciadas.Clear();
    }

    public void ActualizarUI(float distancia, float puntos)
    {
        txtDistancia.text = "Distancia " + distancia.ToString("F1");
        txtPuntos.text = "Puntos acumulados " + puntos;
    }

    public void FinDelJuego(float distancia, float puntos)
    {
        panelFinJuego.SetActive(true);
        txtDistanciaFinal.text = "Distancia recorrida " + "......" + distancia.ToString("F1");
        txtPuntosFinal.text = "Puntos acumulados " + "......" + puntos;

        float total = distancia + puntos;
        txtTotal.text = "Total " + ".........." + total.ToString("F1");
    }

    public void reiniciarJuego()
    {
        _poncho.reiniciar(posicionInicialPoncho.position);
        txtDistancia.text = "Distancia ";
        txtPuntos.text = "Puntos ";

        panelFinJuego.SetActive(false);

        controller.reiniciarEstado();

        limpiarMonedas();
        InstaciarMonedas();
    }

    public void SalirDelJuego()
    {
        Application.Quit();

        // Para probar en editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
