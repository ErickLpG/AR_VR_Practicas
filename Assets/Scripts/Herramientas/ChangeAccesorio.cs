using UnityEngine;

public class ChangeAccesorio : MonoBehaviour
{
    [Header("Accesorios")]
    public GameObject[] accesorios;
    public GameObject[] accesoriosExtra;

    void Start()
    {
        OcultarTodos();
    }

    void Update()
    {
        Actualizaraccesorios();
    }

    public void CambiarEstado()
    {
        int randomValue = Random.Range(0, accesorios.Length);
        Actualizaraccesorios(randomValue);
    }

    public void Actualizaraccesorios()
    {
        for(int i = 0; i < accesorios.Length; i++)
        {
            if(accesorios[i].activeSelf)
            {
                accesoriosExtra[i].SetActive(true);
            }
            else
            {
                accesoriosExtra[i].SetActive(false);
            }
        }
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