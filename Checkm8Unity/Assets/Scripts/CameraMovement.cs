using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _cameraScrollSpeed;
    [SerializeField] private bool _isMoving = false;
    [HideInInspector] public Vector3 _target = Vector3.zero;

    private void Update()
    {
        if (_isMoving)
        {
            if (_target == Vector3.zero)
            {
                // if no camera target set
                transform.Translate(_cameraScrollSpeed * Time.deltaTime * Vector3.forward, Space.World);
            }
            else
            {
                // if camera is set
                transform.position = Vector3.MoveTowards(transform.position, _target, _cameraScrollSpeed * Time.deltaTime);
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public void ResetTarget()
    {
        _target = Vector3.zero;
    }
}
