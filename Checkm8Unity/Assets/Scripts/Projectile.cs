using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rb.angularVelocity = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _rb.AddRelativeForce(new Vector3(0, 0, _speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(1);
        }
    }

    private void OnBecameInvisible()
    {
        
        gameObject.SetActive(false);
    }
}
