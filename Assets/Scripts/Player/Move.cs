using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    [Header("Componentes principales")]
    public GameObject player;
    public ObserverBehaviour[] ImageTargets;
    public Transform cameraTarget;
    public EventManger eventManger;

    [Header("Configuración movimiento")]
    public float speed = 1.0f;
    public float rotationSpeed = 5.0f;
    public float delayAntesDeMover = 2.0f;
    public float duracionGiroFinal = 0.5f;

    private bool isMoving = false;
    private bool esperandoMovimiento = false;

    private Animator animator;
    private bool primerTargetAsignado = false;
    private ObserverBehaviour lastDetectedTarget = null;

    private int visitasCompletadas = 0;

    private HashSet<ObserverBehaviour> targetsVisitados = new HashSet<ObserverBehaviour>();

    void Start()
    {
        if (player != null)
            animator = player.GetComponent<Animator>();

        if (cameraTarget == null && Camera.main != null)
            cameraTarget = Camera.main.transform;
    }

    void Update()
    {
        if (isMoving || esperandoMovimiento)
            return;

        ObserverBehaviour detected = GetDetectedTarget();

        if (detected != null)
        {
            if (!primerTargetAsignado)
            {
                primerTargetAsignado = true;
                lastDetectedTarget = detected;
                StartCoroutine(EsperarYMover(detected));
                return;
            }

            if (detected != lastDetectedTarget && !targetsVisitados.Contains(detected))
            {
                lastDetectedTarget = detected;
                StartCoroutine(EsperarYMover(detected));
            }
        }
    }

    IEnumerator EsperarYMover(ObserverBehaviour target)
    {
        esperandoMovimiento = true;

        Debug.Log("Target detectado, esperando antes de moverse...");

        yield return new WaitForSeconds(delayAntesDeMover);

        esperandoMovimiento = false;

        yield return StartCoroutine(MoveModel(target));
    }

    private IEnumerator MoveModel(ObserverBehaviour target)
    {
        isMoving = true;

        if (animator != null)
            animator.SetBool("isMoving", true);

        if (target == null)
        {
            if (animator != null)
                animator.SetBool("isMoving", false);

            isMoving = false;
            yield break;
        }

        player.transform.SetParent(null, true);

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0f;

        while (journey <= 1f)
        {
            journey += Time.deltaTime * speed;

            player.transform.position = Vector3.Lerp(startPosition, endPosition, journey);

            Vector3 direction = (endPosition - player.transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                player.transform.rotation = Quaternion.Slerp(
                    player.transform.rotation,
                    lookRotation,
                    Time.deltaTime * rotationSpeed
                );
            }

            yield return null;
        }

        player.transform.position = endPosition;
        player.transform.SetParent(target.transform, true);

        if (animator != null)
            animator.SetBool("isMoving", false);

        targetsVisitados.Add(target);

        yield return StartCoroutine(RotarHaciaCamara());

        visitasCompletadas++;
        EjecutarEventoPorVisita();

        isMoving = false;
    }

    private void EjecutarEventoPorVisita()
    {
        Debug.Log("Visitas completadas: " + visitasCompletadas);

        if (visitasCompletadas == 1)
        {
            Debug.Log("Primera visita: inicio de la aventura.");
            return;
        }

        if (visitasCompletadas >= 2 && visitasCompletadas <= 4)
        {
            if (eventManger != null)
                eventManger.AbrirSiguienteMinijuego();

            return;
        }

        if (visitasCompletadas == 5)
        {
            if (eventManger != null)
                eventManger.MostrarDespedida();

            return;
        }
    }

    private IEnumerator RotarHaciaCamara()
    {
        if (player == null || cameraTarget == null)
            yield break;

        Vector3 direccion = cameraTarget.position - player.transform.position;
        direccion.y = 0f;

        if (direccion.sqrMagnitude <= 0.0001f)
            yield break;

        Quaternion rotacionInicial = player.transform.rotation;
        Quaternion rotacionFinal = Quaternion.LookRotation(direccion.normalized);

        float tiempo = 0f;

        while (tiempo < duracionGiroFinal)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionGiroFinal;

            player.transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, t);
            yield return null;
        }

        player.transform.rotation = rotacionFinal;
    }

    private ObserverBehaviour GetDetectedTarget()
    {
        if (ImageTargets == null || ImageTargets.Length == 0)
            return null;

        foreach (var target in ImageTargets)
        {
            if (target == null)
                continue;

            if ((target.TargetStatus.Status == Status.TRACKED ||
                 target.TargetStatus.Status == Status.EXTENDED_TRACKED) &&
                 !targetsVisitados.Contains(target))
            {
                return target;
            }
        }

        return null;
    }
}