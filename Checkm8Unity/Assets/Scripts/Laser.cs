using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(2*Time.deltaTime);
        }

        if (other.CompareTag("Bullet"))
        {
            other.attachedRigidbody.gameObject.SetActive(false);
            // Destroy(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(2 * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(1);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(1);
        }
    }
}
