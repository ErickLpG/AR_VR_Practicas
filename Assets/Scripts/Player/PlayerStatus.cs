using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI texto;

    void Update()
    {
        if(player == null)
            return;

        if(player.gameObject.activeInHierarchy && player.parent != null)
        {
            texto.color = Color.green;
            texto.text = "Ubicación: " + player.parent.name;
        }
        else
        {
            texto.color = Color.red;
            texto.text = "Personaje no detectado";
        }
    }
}