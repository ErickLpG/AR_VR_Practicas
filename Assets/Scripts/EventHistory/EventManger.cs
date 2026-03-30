using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vuforia;

public class EventManger : MonoBehaviour
{
    [Header("Referencias UI")]
    public UnityEngine.UI.Image panelNegro;
    public TextMeshProUGUI textoHistoria;
    public TextMeshProUGUI textoIndicacion;
    public Animator animator;

    [Header("Targets que pueden activar el cambio")]
    public ObserverBehaviour[] targetsEscaneo;

    [Header("Textos")]
    [TextArea(3, 6)]
    public string historia = "Historia.";

    [TextArea(2, 4)]
    public string indicacion = "Escanea cualquiera de los marcadores para comenzar la aventura.";

    [TextArea(2, 4)]
    public string textoDespuesDeEscaneo = "Muy bien. Ahora acompaña al personaje en sus actividades del día.";

    [Header("Velocidades")]
    public float tiempoEntreLetras = 0.04f;
    public float esperaAntesDeFade = 1.2f;
    public float duracionFadePanel = 1.5f;
    public float duracionFadeIndicacion = 1f;

    [Header("Configuración de detección")]
    public bool aceptarLimitedComoDetectado = true;

    [Header("Minijuegos aleatorios")]
    public GameObject[] panelesMinijuego;
    private List<int> ordenMinijuegos = new List<int>();
    private int indiceMinijuegoActual = 0;
    private GameObject panelMinijuegoActivo = null;

    private bool listoParaEscanear = false;
    private bool yaSeDetecto = false;

    private void Start()
    {
        if (panelNegro != null)
        {
            panelNegro.gameObject.SetActive(true);
            SetImageAlpha(panelNegro, 1f);
        }

        StartCoroutine(SecuenciaInicio());
        PrepararOrdenMinijuegos();
        CerrarTodosLosMinijuegos();
    }

    private void Update()
    {
        if (!listoParaEscanear || yaSeDetecto)
            return;

        if (CualquierTargetDetectado())
        {
            yaSeDetecto = true;
            CambiarTextoDespuesDeEscaneo();
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

        listoParaEscanear = true;
    }

    IEnumerator TypeWriter(string mensaje)
    {
        if (textoHistoria == null)
            yield break;

        textoHistoria.text = "";

        foreach (char letra in mensaje)
        {
            textoHistoria.text += letra;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
    }

    IEnumerator FadeOutPanel()
    {
        if (panelNegro == null || textoHistoria == null)
            yield break;

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
        if (texto == null)
            yield break;

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

    bool CualquierTargetDetectado()
    {
        if (targetsEscaneo == null || targetsEscaneo.Length == 0)
            return false;

        for (int i = 0; i < targetsEscaneo.Length; i++)
        {
            ObserverBehaviour target = targetsEscaneo[i];

            if (target == null)
                continue;

            if (TargetEstaDetectado(target.TargetStatus))
                return true;
        }

        return false;
    }

    void CambiarTextoDespuesDeEscaneo()
    {
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
        if (imagen == null)
            return;

        Color c = imagen.color;
        imagen.color = new Color(c.r, c.g, c.b, alpha);
    }

    void SetTextAlpha(TextMeshProUGUI texto, float alpha)
    {
        if (texto == null)
            return;

        Color c = texto.color;
        texto.color = new Color(c.r, c.g, c.b, alpha);
    }
    
    void PrepararOrdenMinijuegos()
    {
        ordenMinijuegos.Clear();

        for (int i = 0; i < panelesMinijuego.Length; i++)
        {
            ordenMinijuegos.Add(i);
        }

        for (int i = 0; i < ordenMinijuegos.Count; i++)
        {
            int randomIndex = Random.Range(i, ordenMinijuegos.Count);
            int temp = ordenMinijuegos[i];
            ordenMinijuegos[i] = ordenMinijuegos[randomIndex];
            ordenMinijuegos[randomIndex] = temp;
        }

        indiceMinijuegoActual = 0;
    }

    public void AbrirSiguienteMinijuego()
    {
        CerrarTodosLosMinijuegos();

        if (panelesMinijuego == null || panelesMinijuego.Length == 0)
            return;

        if (indiceMinijuegoActual >= ordenMinijuegos.Count)
        {
            Debug.Log("Ya se usaron todos los minijuegos.");
            return;
        }

        int indicePanel = ordenMinijuegos[indiceMinijuegoActual];
        panelMinijuegoActivo = panelesMinijuego[indicePanel];

        if (panelMinijuegoActivo != null)
            panelMinijuegoActivo.SetActive(true);

        indiceMinijuegoActual++;
    }

    public void FinalizarMinijuegoActual()
    {
        if (panelMinijuegoActivo != null)
            panelMinijuegoActivo.SetActive(false);

        panelMinijuegoActivo = null;
    }

    void CerrarTodosLosMinijuegos()
    {
        if (panelesMinijuego == null)
            return;

        for (int i = 0; i < panelesMinijuego.Length; i++)
        {
            if (panelesMinijuego[i] != null)
                panelesMinijuego[i].SetActive(false);
        }
    }

    public void MostrarDespedida()
    {
        CerrarTodosLosMinijuegos();
        animator.SetBool("isDancing", true);
        
        if (textoIndicacion != null)
            textoIndicacion.text = "Gracias por acompañar al personaje en su rutina de hoy. Si quieres repetir el día solo presiona el botón de reiniciar!";
    }
}