using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 100f;

    private bool rotateLeftPressed = false;
    private bool rotateRightPressed = false;

    void Update()
    {
        if (rotateLeftPressed)
        {
            player.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        if (rotateRightPressed)
        {
            player.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnLeftDown()
    {
        rotateLeftPressed = true;
    }

    public void OnLeftUp()
    {
        rotateLeftPressed = false;
    }

    public void OnRightDown()
    {
        rotateRightPressed = true;
    }

    public void OnRightUp()
    {
        rotateRightPressed = false;
    }
}