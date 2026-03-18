using System.Collections;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    [Header("Componentes principales")]
    public GameObject player;
    public ObserverBehaviour[] ImageTargets;
    public int currentTarget;
    public float speed = 1.0f;
    private bool isMoving = false;

    [Header("Componentes para animación")]
    private Animator animator;

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

    }
    #endregion

    #region Movimiento
    public void moveToNextMarker()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }

    private IEnumerator MoveModel()
    {
        isMoving = true;
        ObserverBehaviour target = GetNextDetectedTarget();
        
        // Activar animación de movimiento
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }

        if(target == null)
        {
            isMoving = false;
            yield break;
        }

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0f;

        while(journey <= 1f)
        {
            journey += Time.deltaTime * speed;
            player.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }

        currentTarget = (currentTarget + 1) % ImageTargets.Length;
        
        // Desactivar animación de movimiento
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        isMoving = false;
    }

    private ObserverBehaviour GetNextDetectedTarget()
    {
        foreach(ObserverBehaviour target in ImageTargets)
        {
            if(target != null && (target.TargetStatus.Status == Status.TRACKED || target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                return target;
            }
        }
        return null;
    }
    #endregion
}
