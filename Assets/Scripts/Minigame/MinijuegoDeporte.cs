using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoDeporte : MonoBehaviour
{
    [Header("UI principal")]
    public Slider barraPotencia;
    public TextMeshProUGUI textoIndicacion;
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoContador;
    public Button botonGolpear;
    public Button botonReiniciar;

    [Header("Panel final")]
    public GameObject panelPuntuacionFinal;
    public TextMeshProUGUI textoPuntuacionFinal;
    public Image estrella1;
    public Image estrella2;
    public Image estrella3;

    [Header("Configuracion")]
    public float velocidadBarra = 1.2f;
    [Range(0f, 1f)] public float zonaMinima = 0.45f;
    [Range(0f, 1f)] public float zonaMaxima = 0.65f;
    public int totalTiros = 5;

    [Header("Colores opcionales")]
    public Image fillBarra;
    public Color colorNormal = Color.white;
    public Color colorAcierto = Color.green;
    public Color colorFallo = Color.red;
    public Color colorEstrellaActiva = Color.yellow;
    public Color colorEstrellaInactiva = Color.gray;

    private bool juegoActivo = false;
    private bool subiendo = true;
    private bool resultadoMostrado = false;

    private int tirosRealizados = 0;
    private int aciertos = 0;

    void Start()
    {
        if (botonGolpear != null)
            botonGolpear.onClick.AddListener(Golpear);

        if (botonReiniciar != null)
            botonReiniciar.onClick.AddListener(ReiniciarJuego);

        ReiniciarJuego();
    }

    void Update()
    {
        if (!juegoActivo || barraPotencia == null)
            return;

        float cambio = velocidadBarra * Time.deltaTime;

        if (subiendo)
        {
            barraPotencia.value += cambio;

            if (barraPotencia.value >= 1f)
            {
                barraPotencia.value = 1f;
                subiendo = false;
            }
        }
        else
        {
            barraPotencia.value -= cambio;

            if (barraPotencia.value <= 0f)
            {
                barraPotencia.value = 0f;
                subiendo = true;
            }
        }
    }

    public void IniciarJuego()
    {
        if (barraPotencia == null)
            return;

        panelPuntuacionFinal.SetActive(false);

        barraPotencia.value = 0f;
        subiendo = true;
        juegoActivo = true;
        resultadoMostrado = false;

        if (textoIndicacion != null)
            textoIndicacion.text = "Presiona cuando la barra esté en la zona correcta";

        if (textoResultado != null)
            textoResultado.text = "";

        ActualizarContador();

        if (fillBarra != null)
            fillBarra.color = colorNormal;

        if (botonGolpear != null)
            botonGolpear.interactable = true;
    }

    public void Golpear()
    {
        if (!juegoActivo || barraPotencia == null || resultadoMostrado)
            return;

        juegoActivo = false;
        resultadoMostrado = true;
        tirosRealizados++;

        float valor = barraPotencia.value;
        bool fueAcierto = valor >= zonaMinima && valor <= zonaMaxima;

        if (fueAcierto)
        {
            aciertos++;

            if (textoResultado != null)
                textoResultado.text = "Buen golpe";

            if (fillBarra != null)
                fillBarra.color = colorAcierto;
        }
        else
        {
            if (textoResultado != null)
                textoResultado.text = "Fallaste";

            if (fillBarra != null)
                fillBarra.color = colorFallo;
        }

        ActualizarContador();

        if (botonGolpear != null)
            botonGolpear.interactable = false;

        StartCoroutine(SiguienteTiroAutomatico());
    }

    public void SiguienteTiro()
    {
        if (tirosRealizados >= totalTiros)
            return;

        IniciarJuego();
    }

    IEnumerator SiguienteTiroAutomatico()
    {
        yield return new WaitForSeconds(1f);

        if (tirosRealizados >= totalTiros)
        {
            MostrarPanelFinal();
        }
        else
        {
            IniciarJuego();
        }
    }

    public void ReiniciarJuego()
    {
        tirosRealizados = 0;
        aciertos = 0;

        if (panelPuntuacionFinal != null)
            panelPuntuacionFinal.SetActive(false);

        ResetearEstrellas();
        IniciarJuego();
    }

    void ActualizarContador()
    {
        if (textoContador != null)
            textoContador.text = "Tiros restantes: " + tirosRealizados + "/" + totalTiros;
    }

    void MostrarPanelFinal()
    {
        if (panelPuntuacionFinal != null)
            panelPuntuacionFinal.SetActive(true);

        if (textoPuntuacionFinal != null)
            textoPuntuacionFinal.text = "Aciertos: " + aciertos + "/" + totalTiros;

        int estrellasGanadas = CalcularEstrellas(aciertos);
        ActualizarEstrellas(estrellasGanadas);
    }

    int CalcularEstrellas(int totalAciertos)
    {
        if (totalAciertos >= 5)
            return 3;

        if (totalAciertos >= 3)
            return 2;

        if (totalAciertos >= 1)
            return 1;

        return 0;
    }

    void ActualizarEstrellas(int cantidad)
    {
        if (estrella1 != null)
            estrella1.color = cantidad >= 1 ? colorEstrellaActiva : colorEstrellaInactiva;

        if (estrella2 != null)
            estrella2.color = cantidad >= 2 ? colorEstrellaActiva : colorEstrellaInactiva;

        if (estrella3 != null)
            estrella3.color = cantidad >= 3 ? colorEstrellaActiva : colorEstrellaInactiva;
    }

    void ResetearEstrellas()
    {
        if (estrella1 != null)
            estrella1.color = colorEstrellaInactiva;

        if (estrella2 != null)
            estrella2.color = colorEstrellaInactiva;

        if (estrella3 != null)
            estrella3.color = colorEstrellaInactiva;
    }
}