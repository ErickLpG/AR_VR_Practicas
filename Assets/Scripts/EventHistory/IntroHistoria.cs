using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class IntroHistoria : MonoBehaviour
{
    [Header("Referencias UI")]
    public UnityEngine.UI.Image panelNegro;
    public TextMeshProUGUI textoHistoria;
    public TextMeshProUGUI textoIndicacion;
    public UnityEngine.UI.Image imagenGuia;

    [Header("Target que activará el cambio")]
    public ObserverBehaviour targetEscaneo;

    [Header("Textos")]
    [TextArea(3, 6)]
    public string historia = "El mundo está en peligro.\nTú no puedes intervenir directamente en este lugar...\nPero puedes guiar a un héroe para salvarlo.";

    public string indicacion = "Escanea la siguiente imagen con la cámara de tu celular";
    public string textoDespuesDeEscaneo = "Bien. Has encontrado al héroe. Ahora guíalo hacia otro destino.";

    [Header("Velocidades")]
    public float tiempoEntreLetras = 0.04f;
    public float esperaAntesDeFade = 1.2f;
    public float duracionFadePanel = 1.5f;
    public float duracionFadeIndicacion = 1f;
    public float duracionFadeImagen = 1f;

    [Header("Configuración de detección")]
    public bool aceptarLimitedComoDetectado = true;

    private bool listoParaEscanear = false;
    private bool yaSeDetecto = false;

    private void Start()
    {
        panelNegro.gameObject.SetActive(true);

        if (imagenGuia != null)
        {
            imagenGuia.gameObject.SetActive(true);
            SetImageAlpha(imagenGuia, 0f);
        }

        StartCoroutine(SecuenciaInicio());
    }

    private void Update()
    {
        if (!listoParaEscanear || yaSeDetecto || targetEscaneo == null)
            return;

        if (TargetEstaDetectado(targetEscaneo.TargetStatus))
        {
            yaSeDetecto = true;
            StartCoroutine(OcultarImagenYCambiarTexto());
        }
    }

    IEnumerator SecuenciaInicio()
    {
        if (textoHistoria != null)
            textoHistoria.text = "";

        if (textoIndicacion != null)
        {
            textoIndicacion.text = "";
            SetTextAlpha(textoIndicacion, 0f);
        }

        if (panelNegro != null)
            SetImageAlpha(panelNegro, 1f);

        yield return StartCoroutine(TypeWriter(historia));

        yield return new WaitForSeconds(esperaAntesDeFade);

        yield return StartCoroutine(FadeOutPanel());

        if (textoHistoria != null)
            textoHistoria.text = "";

        if (textoIndicacion != null)
        {
            textoIndicacion.text = indicacion;
            yield return StartCoroutine(FadeInText(textoIndicacion, duracionFadeIndicacion));
        }

        if (imagenGuia != null)
        {
            yield return StartCoroutine(FadeInImage(imagenGuia, duracionFadeIndicacion));
        }

        listoParaEscanear = true;
    }

    IEnumerator TypeWriter(string mensaje)
    {
        textoHistoria.text = "";

        foreach (char letra in mensaje)
        {
            textoHistoria.text += letra;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
    }

    IEnumerator FadeOutPanel()
    {
        float tiempo = 0f;

        Color colorPanel = panelNegro.color;
        Color colorTexto = textoHistoria.color;

        while (tiempo < duracionFadePanel)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionFadePanel;
            float alpha = Mathf.Lerp(1f, 0f, t);

            panelNegro.color = new Color(colorPanel.r, colorPanel.g, colorPanel.b, alpha);
            textoHistoria.color = new Color(colorTexto.r, colorTexto.g, colorTexto.b, alpha);

            yield return null;
        }

        panelNegro.color = new Color(colorPanel.r, colorPanel.g, colorPanel.b, 0f);
        textoHistoria.color = new Color(colorTexto.r, colorTexto.g, colorTexto.b, 0f);
    }

    IEnumerator FadeInText(TextMeshProUGUI texto, float duracion)
    {
        float tiempo = 0f;
        Color colorBase = texto.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            texto.color = new Color(colorBase.r, colorBase.g, colorBase.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        texto.color = new Color(colorBase.r, colorBase.g, colorBase.b, 1f);
    }

    IEnumerator FadeInImage(UnityEngine.UI.Image imagen, float duracion)
    {
        float tiempo = 0f;
        Color colorBase = imagen.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            imagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        imagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, 1f);
    }

    IEnumerator FadeOutImage(UnityEngine.UI.Image imagen, float duracion)
    {
        float tiempo = 0f;
        Color colorBase = imagen.color;
        float alphaInicial = imagen.color.a;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            imagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, Mathf.Lerp(alphaInicial, 0f, t));
            yield return null;
        }

        imagen.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);
        imagen.gameObject.SetActive(false);
    }

    IEnumerator OcultarImagenYCambiarTexto()
    {
        if (imagenGuia != null && imagenGuia.gameObject.activeSelf)
        {
            yield return StartCoroutine(FadeOutImage(imagenGuia, duracionFadeImagen));
        }

        if (textoIndicacion != null)
        {
            textoIndicacion.text = textoDespuesDeEscaneo;
        }
    }

    bool TargetEstaDetectado(TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
            return true;

        if (aceptarLimitedComoDetectado && targetStatus.Status == Status.LIMITED)
            return true;

        return false;
    }

    void SetImageAlpha(UnityEngine.UI.Image imagen, float alpha)
    {
        Color c = imagen.color;
        imagen.color = new Color(c.r, c.g, c.b, alpha);
    }

    void SetTextAlpha(TextMeshProUGUI texto, float alpha)
    {
        Color c = texto.color;
        texto.color = new Color(c.r, c.g, c.b, alpha);
    }
}