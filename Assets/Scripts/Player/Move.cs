using System.Collections;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    [Header("Componentes principales")]
    public GameObject player;
    public ObserverBehaviour playerTarget;

    public ObserverBehaviour[] ImageTargets;     // Targets de destino

    public int currentTarget = 0;
    public float speed = 1.0f;
    private bool isMoving = false;

    [Header("Componentes para animación")]
    private Animator animator;

    [Header("Rotación")]
    public float rotationSpeed = 5.0f;
    //private bool returningToPlayerTarget = false;

    private ObserverBehaviour currentMoveTarget = null;
    private ObserverBehaviour lastDetectedTarget = null;

    #region Métodos Unity
    void Start()
    {
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Si está en movimiento, no hacer nada
        if (isMoving) return;

        ObserverBehaviour detected = GetDetectedTarget();

        // Si detecta uno nuevo distinto al anterior
        if (detected != null && detected != lastDetectedTarget)
        {
            lastDetectedTarget = detected;
            StartCoroutine(MoveModel(detected));
        }
    }
    #endregion

    #region Movimiento
    /*
    public void MoveButton()
    {
        // Si no se está moviendo, revisa si hay un nuevo target disponible.
        if (!isMoving)
        {
            ObserverBehaviour nextTarget = GetNextDetectedTarget();

            // Si hay un target válido y es diferente al actual, inicia movimiento.
            if (nextTarget != null)
            {
                moveToNextMarker();
            }
        }
    }

    public void moveToNextMarker()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }
    */

    private IEnumerator MoveModel(ObserverBehaviour target)
    {
        isMoving = true;
        currentMoveTarget = target;

        // Activar animación de movimiento
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }

        if (target == null)
        {
            // Apagamos animación también si no hay target.
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }

            isMoving = false;
            yield break;
        }

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0f;

        while (journey <= 1f)
        {
            journey += Time.deltaTime * speed;

            // Movimiento
            player.transform.position = Vector3.Lerp(startPosition, endPosition, journey);

            // Rotar al player hacia la dirección del objetivo mientras se mueve.
            Vector3 direction = (endPosition - player.transform.position).normalized;

            // Evita errores si la dirección es muy pequeña.
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

        // Aseguramos la posición final exacta.
        player.transform.position = endPosition;

        /*
        // Si no está regresando al playerTarget, avanza al siguiente destino.
        if (!returningToPlayerTarget)
        {
            currentTarget++;

            // Si ya terminó todos los destinos, ahora regresa al target original del player.
            if (currentTarget >= ImageTargets.Length)
            {
                currentTarget = 0;
                returningToPlayerTarget = true;
            }
        }
        else
        {
            // Si ya regresó al target original, termina el ciclo.
            returningToPlayerTarget = false;
        }
        */
        
        // Desactivar animación de movimiento
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        isMoving = false;
    }

    private ObserverBehaviour GetDetectedTarget()
    {
        if (ImageTargets == null || ImageTargets.Length == 0)
            return null;

        foreach (var target in ImageTargets)
        {
            if (target != null &&
                (target.TargetStatus.Status == Status.TRACKED ||
                 target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                return target;
            }
        }

        return null;
    }
    
    /*
    private ObserverBehaviour GetNextDetectedTarget()
    {
        // Si estamos en la fase de regreso, intentamos volver al playerTarget.
        if (returningToPlayerTarget)
        {
            if (playerTarget != null &&
                playerTarget.TargetStatus.Status == Status.TRACKED)
            {
                Debug.Log($"Regresando al playerTarget: {playerTarget.TargetName}");
                return playerTarget;
            }

            Debug.Log("playerTarget no detectado, se continuará con ImageTargets");
            returningToPlayerTarget = false;
        }

        // Si hay destinos, buscamos primero el actual según currentTarget.
        if (ImageTargets != null && ImageTargets.Length > 0)
        {
            // Recorremos circularmente desde currentTarget para encontrar el siguiente detectado.
            for (int i = 0; i < ImageTargets.Length; i++)
            {
                int index = (currentTarget + i) % ImageTargets.Length;
                ObserverBehaviour target = ImageTargets[index];

                if (target != null &&
                    target.TargetStatus.Status == Status.TRACKED)
                {
                    Debug.Log($"Target detectado: {target.TargetName}");
                    return target;
                }
            }
        }
        return null;
    }
    */
    #endregion
}