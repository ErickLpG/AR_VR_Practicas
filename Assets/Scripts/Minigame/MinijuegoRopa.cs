using UnityEngine;

public class MinijuegoRopa : MonoBehaviour
{
    public GameObject PanelRopaExtra;

    void Start()
    {
        PanelRopaExtra.SetActive(true);
    }
    
    public void terminarMinijuego()
    {
        PanelRopaExtra.SetActive(false);
    }
}
