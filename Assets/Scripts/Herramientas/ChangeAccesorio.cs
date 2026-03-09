using UnityEngine;

public class ChangeAccesorio : MonoBehaviour
{
    public GameObject[] objetos;
    public ManageAnimations manageAnimations;

    private int indiceActual = -1;

    void Start()
    {
        OcultarTodos();
    }

    public void CambiarEstado()
    {
        indiceActual++;

        if (indiceActual >= objetos.Length)
        {
            indiceActual = -1; 
        }

        ActualizarObjetos();

        if (manageAnimations != null)
        {
            manageAnimations.SetIdle();
        }
    }

    public void OcultarTodos()
    {
        indiceActual = -1;

        for (int i = 0; i < objetos.Length; i++)
        {
            objetos[i].SetActive(false);
        }
    }

    void ActualizarObjetos()
    {
        for (int i = 0; i < objetos.Length; i++)
        {
            objetos[i].SetActive(i == indiceActual);
        }
    }
}