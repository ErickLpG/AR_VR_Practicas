using UnityEngine;

public class ChangeAccesorio : MonoBehaviour
{
    [Header("Accesorios")]
    public GameObject[] accesorios;

    void Start()
    {
        OcultarTodos();
    }

    public void CambiarEstado()
    {
        int randomValue = Random.Range(0, accesorios.Length);
        Actualizaraccesorios(randomValue);
    }

    public void OcultarTodos()
    {
        for (int i = 0; i < accesorios.Length; i++)
        {
            accesorios[i].SetActive(false);
        }
    }

    void Actualizaraccesorios(int randomValue)
    {
        for (int i = 0; i < accesorios.Length; i++)
        {
            accesorios[i].SetActive(i == randomValue);
        }
    }
}