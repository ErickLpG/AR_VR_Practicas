using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoPiano : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoIndicacion;
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoProgreso;
    public Button botonReiniciar;

    [Header("Teclas visuales")]
    public Image teclaDo;
    public Image teclaRe;
    public Image teclaMi;
    public Image teclaFa;
    public Image teclaSol;
    public Image teclaLa;
    public Image teclaSi;
    public Color colorNormalTecla = Color.white;
    public Color colorActivaTecla = Color.yellow;
    public float tiempoResaltado = 0.35f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip notaDo;
    public AudioClip notaRe;
    public AudioClip notaMi;
    public AudioClip notaFa;
    public AudioClip notaSol;
    public AudioClip notaLa;
    public AudioClip notaSi;

    [Header("Configuracion")]
    public int longitudSecuencia = 3;
    public bool secuenciaAleatoria = true;
    public float tiempoAntesDeIniciar = 3f;

    private bool mostrandoSecuencia = false;
    private bool minijuegoActivo = false;
    private bool modoLibre = false;

    private List<int> secuenciaObjetivo = new List<int>();
    private List<int> secuenciaJugador = new List<int>();

    void Start()
    {
        if (botonReiniciar != null)
            botonReiniciar.onClick.AddListener(ReiniciarJuego);

        ResetearColoresTeclas();
        ReiniciarJuego();
    }

    public void ReiniciarJuego()
    {
        StopAllCoroutines();

        secuenciaObjetivo.Clear();
        secuenciaJugador.Clear();

        ResetearColoresTeclas();
        GenerarSecuencia();
        ActualizarTextoProgreso();

        if (textoResultado != null)
            textoResultado.text = "";

        if (textoIndicacion != null)
            textoIndicacion.text = "Prepárate...";

        minijuegoActivo = false;
        mostrandoSecuencia = false;
        modoLibre = false;

        StartCoroutine(MostrarSecuencia());
    }

    void GenerarSecuencia()
    {
        if (secuenciaAleatoria)
        {
            int ultimaNota = -1;

            for (int i = 0; i < longitudSecuencia; i++)
            {
                int nuevaNota;

                do
                {
                    nuevaNota = Random.Range(0, 7);
                }
                while (nuevaNota == ultimaNota);

                secuenciaObjetivo.Add(nuevaNota);
                ultimaNota = nuevaNota;
            }
        }
        else
        {
            secuenciaObjetivo.Add(0); 
            secuenciaObjetivo.Add(2);
            secuenciaObjetivo.Add(4);

            if (longitudSecuencia > 3)
            {
                int ultimaNota = 4;

                for (int i = 3; i < longitudSecuencia; i++)
                {
                    int nuevaNota;

                    do
                    {
                        nuevaNota = Random.Range(0, 7);
                    }
                    while (nuevaNota == ultimaNota);

                    secuenciaObjetivo.Add(nuevaNota);
                    ultimaNota = nuevaNota;
                }
            }
        }
    }

    IEnumerator MostrarSecuencia()
    {
        mostrandoSecuencia = true;
        minijuegoActivo = false;

        if (textoIndicacion != null)
            textoIndicacion.text = "A continuación, sigue la secuencia de notas para completar el minijuego.";

        yield return new WaitForSeconds(tiempoAntesDeIniciar);

        if (textoIndicacion != null)
            textoIndicacion.text = "Observa la secuencia";

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < secuenciaObjetivo.Count; i++)
        {
            yield return StartCoroutine(ReproducirNotaVisual(secuenciaObjetivo[i]));
            yield return new WaitForSeconds(0.2f);
        }

        mostrandoSecuencia = false;
        minijuegoActivo = true;

        if (textoIndicacion != null)
            textoIndicacion.text = "Ahora repítela";
    }

    IEnumerator ReproducirNotaVisual(int indice)
    {
        Image tecla = ObtenerTecla(indice);
        AudioClip clip = ObtenerClip(indice);

        if (tecla != null)
            tecla.color = colorActivaTecla;

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        yield return new WaitForSeconds(tiempoResaltado);

        if (tecla != null)
            tecla.color = colorNormalTecla;
    }

    IEnumerator ResaltarTeclaJugador(Image tecla)
    {
        if (tecla == null)
            yield break;

        tecla.color = colorActivaTecla;
        yield return new WaitForSeconds(tiempoResaltado);
        tecla.color = colorNormalTecla;
    }

    public void TocarDo()
    {
        RegistrarNota(0, "Do");
    }

    public void TocarRe()
    {
        RegistrarNota(1, "Re");
    }

    public void TocarMi()
    {
        RegistrarNota(2, "Mi");
    }

    public void TocarFa()
    {
        RegistrarNota(3, "Fa");
    }

    public void TocarSol()
    {
        RegistrarNota(4, "Sol");
    }

    public void TocarLa()
    {
        RegistrarNota(5, "La");
    }

    public void TocarSi()
    {
        RegistrarNota(6, "Si");
    }

    void RegistrarNota(int indiceNota, string nombreNota)
    {
        if (mostrandoSecuencia)
            return;

        AudioClip clip = ObtenerClip(indiceNota);
        Image tecla = ObtenerTecla(indiceNota);

        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        if (tecla != null)
            StartCoroutine(ResaltarTeclaJugador(tecla));

        if (modoLibre)
        {
            if (textoResultado != null)
                textoResultado.text = "Modo libre";

            return;
        }

        if (!minijuegoActivo)
            return;

        secuenciaJugador.Add(indiceNota);

        int indiceActual = secuenciaJugador.Count - 1;

        if (indiceActual >= secuenciaObjetivo.Count)
            return;

        if (secuenciaJugador[indiceActual] != secuenciaObjetivo[indiceActual])
        {
            minijuegoActivo = false;

            if (textoResultado != null)
                textoResultado.text = "Secuencia incorrecta";

            return;
        }

        if (textoResultado != null)
            textoResultado.text = "Nota correcta: " + nombreNota;

        ActualizarTextoProgreso();

        if (secuenciaJugador.Count == secuenciaObjetivo.Count)
        {
            minijuegoActivo = false;
            modoLibre = true;

            if (textoResultado != null)
                textoResultado.text = "Melodía completada";

            if (textoIndicacion != null)
                textoIndicacion.text = "Puedes seguir tocando libremente";
        }
    }

    void ActualizarTextoProgreso()
    {
        if (textoProgreso != null)
            textoProgreso.text = secuenciaJugador.Count + "/" + secuenciaObjetivo.Count;
    }

    AudioClip ObtenerClip(int indice)
    {
        switch (indice)
        {
            case 0: return notaDo;
            case 1: return notaRe;
            case 2: return notaMi;
            case 3: return notaFa;
            case 4: return notaSol;
            case 5: return notaLa;
            case 6: return notaSi;
        }

        return null;
    }

    Image ObtenerTecla(int indice)
    {
        switch (indice)
        {
            case 0: return teclaDo;
            case 1: return teclaRe;
            case 2: return teclaMi;
            case 3: return teclaFa;
            case 4: return teclaSol;
            case 5: return teclaLa;
            case 6: return teclaSi;
        }

        return null;
    }

    void ResetearColoresTeclas()
    {
        if (teclaDo != null) teclaDo.color = colorNormalTecla;
        if (teclaRe != null) teclaRe.color = colorNormalTecla;
        if (teclaMi != null) teclaMi.color = colorNormalTecla;
        if (teclaFa != null) teclaFa.color = colorNormalTecla;
        if (teclaSol != null) teclaSol.color = colorNormalTecla;
        if (teclaLa != null) teclaLa.color = colorNormalTecla;
        if (teclaSi != null) teclaSi.color = colorNormalTecla;
    }
}