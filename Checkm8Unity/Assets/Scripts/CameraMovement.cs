using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _cameraScrollSpeed;
    [SerializeField] private bool _isMoving = false;

    private void Update()
    {
        if (_isMoving)
        {
            transform.Translate(_cameraScrollSpeed * Time.deltaTime * Vector3.forward, Space.World);
        }
    }
}
