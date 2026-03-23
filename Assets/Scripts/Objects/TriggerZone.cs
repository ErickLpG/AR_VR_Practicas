using UnityEngine;
using Vuforia;

public class TriggerZone : MonoBehaviour
{
    [Header("Target de zona")]
    public ObserverBehaviour parentTarget;

    [Header("Tag del personaje")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Transform playerTransform = other.transform;

        playerTransform.SetParent(parentTarget.transform, true);

        Debug.Log($"{other.name} ahora es hijo de {parentTarget.TargetName}");
    }
}