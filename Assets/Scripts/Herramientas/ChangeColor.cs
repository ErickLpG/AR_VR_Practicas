using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    
    [Header("Componentes")]
    public GameObject Modelo;
    public Color color;
    public Material[] colorMaterialBody;

    public void CambiarColor()
    {
        Debug.Log("Cambiando color...");
        
        //Modelo.GetComponent<Renderer>().material.color = color;
        
        for (int i = 0; i < colorMaterialBody.Length; i++)
        {
            colorMaterialBody[i].color = color;
        }
    }

    public void CambiarColorAleatorio()
    {
        Debug.Log("Cambiando color aleatorio...");

        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        Color colorRandom = new Color(red, green, blue);

        //Modelo.GetComponent<Renderer>().material.color = colorRandom;
        
        for (int i = 0; i < colorMaterialBody.Length; i++)
        {
            colorMaterialBody[i].color = colorRandom;
        }
    }
}
