using UnityEngine;

public class SwordModuleTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    public SwordStabilityMinigame minijuegoEspada;

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaSeActivo)
            return;

        if (!other.CompareTag(playerTag))
            return;

        yaSeActivo = true;

        if (minijuegoEspada != null)
            minijuegoEspada.IniciarMinijuego();
    }
}