using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraScrollSpeed;
    [SerializeField] private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(cameraScrollSpeed * Time.deltaTime * Vector3.forward, Space.World);
        }
    }
}
