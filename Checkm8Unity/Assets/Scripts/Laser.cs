using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 2; // so i can choose how much dmg to do per obj

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(damage * Time.deltaTime);
        }
        
        if (other.CompareTag("Bullet"))
        {
            other.attachedRigidbody.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(damage * Time.deltaTime);
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
