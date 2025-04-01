using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private GameManager _gameManager;
    private GameObject _target;
    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _target = GameManager.s_Player;
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = _target.transform;
        Vector3 direction = target.position - _rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rb.angularVelocity = Vector3.up * -rotateAmount * _rotateSpeed;

        _rb.linearVelocity = transform.up * _moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(1);
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
