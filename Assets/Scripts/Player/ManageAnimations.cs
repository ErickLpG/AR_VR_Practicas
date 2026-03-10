using UnityEngine;

public class ManageAnimations : MonoBehaviour
{
    public Animator animator;

    private int estadoActual = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CambiarAnimacion()
    {
        estadoActual++;

        if (estadoActual > 2)
            estadoActual = 0;

        switch (estadoActual)
        {
            case 0: // Idle
                animator.SetBool("isMoving", false);
                animator.SetBool("isDancing", false);
                Debug.Log($"Estado idle: {estadoActual}");
                break;

            case 1: // Run
                animator.SetBool("isMoving", true);
                animator.SetBool("isDancing", false);
                Debug.Log($"Estado run: {estadoActual}");
                break;

            case 2: // Dance
                animator.SetBool("isMoving", false);
                animator.SetBool("isDancing", true);
                Debug.Log($"Estado dance: {estadoActual}");
                break;
        }
    }

    public void SetIdle()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isDancing", false);
    }
}