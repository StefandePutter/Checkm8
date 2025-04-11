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
        // give forward velocity
        _rb.angularVelocity = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _rb.AddRelativeForce(new Vector3(0, 0, _speed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // try damaging collision
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(1);
        }

        gameObject.SetActive(false);
    }
    
    // disable bullet of screen
    private void OnBecameInvisible()
    {
        
        gameObject.SetActive(false);
    }
}
