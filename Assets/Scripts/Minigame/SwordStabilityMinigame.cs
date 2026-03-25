using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordStabilityMinigame : MonoBehaviour
{
    [Header("Paneles y UI")]
    public GameObject panelMinijuego;
    public Button botonEstabilizar;
    public Image barraEstabilidad;
    public TextMeshProUGUI textoInstruccion;
    public TextMeshProUGUI textoEstado;

    [Header("Medalla")]
    public Image medallaImagen;
    public float duracionFadeMedalla = 1f;

    [Header("Configuracion del minijuego")]
    [Range(0f, 1f)]
    public float estabilidadInicial = 0.5f;

    public float velocidadDescenso = 0.25f;
    public float fuerzaPorToque = 0.12f;
    public float tiempoObjetivo = 5f;

    [Header("Zona segura")]
    [Range(0f, 1f)]
    public float zonaSeguraMin = 0.35f;

    [Range(0f, 1f)]
    public float zonaSeguraMax = 0.75f;

    [Header("Mensajes")]
    [TextArea(2, 4)]
    public string mensajeInicio = "Has encontrado la espada legendaria, pero para reconocer que a quien controlas es digno, ayudalo a superar la siguiente prueba.";

    public string mensajeDurante = "Presiona el boton para estabilizar el poder de la espada.";
    public string mensajeVictoria = "La espada ha reconocido al heroe.";
    public string mensajeDerrota = "La espada aun no lo considera digno. Intentalo de nuevo.";

    private float estabilidadActual;
    private float tiempoAcumuladoEnZona;
    private bool minijuegoActivo;
    private bool minijuegoTerminado;

    private void Start()
    {
        if (panelMinijuego != null)
            panelMinijuego.SetActive(false);

        if (medallaImagen != null)
        {
            medallaImagen.gameObject.SetActive(false);
            SetImageAlpha(medallaImagen, 0f);
        }

        if (botonEstabilizar != null)
            botonEstabilizar.onClick.AddListener(EstabilizarEspada);
    }

    private void Update()
    {
        if (!minijuegoActivo || minijuegoTerminado)
            return;

        estabilidadActual -= velocidadDescenso * Time.deltaTime;
        estabilidadActual = Mathf.Clamp01(estabilidadActual);

        if (barraEstabilidad != null)
            barraEstabilidad.fillAmount = estabilidadActual;

        if (estabilidadActual >= zonaSeguraMin && estabilidadActual <= zonaSeguraMax)
        {
            tiempoAcumuladoEnZona += Time.deltaTime;

            if (textoEstado != null)
                textoEstado.text = "Control estable: " + tiempoAcumuladoEnZona.ToString("F1") + " / " + tiempoObjetivo.ToString("F1") + " s";
        }
        else
        {
            if (textoEstado != null)
                textoEstado.text = "La espada esta inestable";
        }

        if (tiempoAcumuladoEnZona >= tiempoObjetivo)
        {
            Victoria();
        }

        if (estabilidadActual <= 0f)
        {
            Derrota();
        }
    }

    public void IniciarMinijuego()
    {
        if (panelMinijuego != null)
            panelMinijuego.SetActive(true);

        estabilidadActual = estabilidadInicial;
        tiempoAcumuladoEnZona = 0f;
        minijuegoActivo = true;
        minijuegoTerminado = false;

        if (barraEstabilidad != null)
            barraEstabilidad.fillAmount = estabilidadActual;

        if (textoInstruccion != null)
            textoInstruccion.text = mensajeInicio + "\n\n" + mensajeDurante;

        if (textoEstado != null)
            textoEstado.text = "Comienza la prueba";
    }

    public void EstabilizarEspada()
    {
        if (!minijuegoActivo || minijuegoTerminado)
            return;

        estabilidadActual += fuerzaPorToque;
        estabilidadActual = Mathf.Clamp01(estabilidadActual);

        if (barraEstabilidad != null)
            barraEstabilidad.fillAmount = estabilidadActual;
    }

    private void Victoria()
    {
        minijuegoActivo = false;
        minijuegoTerminado = true;

        if (textoEstado != null)
            textoEstado.text = mensajeVictoria;

        if (botonEstabilizar != null)
            botonEstabilizar.interactable = false;

        if (medallaImagen != null)
            StartCoroutine(MostrarMedallaConFade());

        StartCoroutine(CerrarPanelDespues(1.5f));
    }

    private void Derrota()
    {
        minijuegoActivo = false;
        minijuegoTerminado = true;

        if (textoEstado != null)
            textoEstado.text = mensajeDerrota;

        if (botonEstabilizar != null)
            botonEstabilizar.interactable = false;
    }

    public void ReintentarMinijuego()
    {
        if (botonEstabilizar != null)
            botonEstabilizar.interactable = true;

        IniciarMinijuego();
    }

    IEnumerator MostrarMedallaConFade()
    {
        medallaImagen.gameObject.SetActive(true);

        float tiempo = 0f;
        Color colorBase = medallaImagen.color;

        while (tiempo < duracionFadeMedalla)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFadeMedalla);
            medallaImagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, alpha);
            yield return null;
        }

        medallaImagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, 1f);
    }

    IEnumerator CerrarPanelDespues(float espera)
    {
        yield return new WaitForSeconds(espera);

        if (panelMinijuego != null)
            panelMinijuego.SetActive(false);
    }

    void SetImageAlpha(Image imagen, float alpha)
    {
        Color c = imagen.color;
        imagen.color = new Color(c.r, c.g, c.b, alpha);
    }
}