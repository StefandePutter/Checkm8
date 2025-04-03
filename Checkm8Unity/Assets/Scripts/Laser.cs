using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 2; // so i can choose how much dmg to do per obj

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(2*Time.deltaTime);
        }
        Debug.Log("hit " + other.name);

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("hit bullet");
            other.attachedRigidbody.gameObject.SetActive(false);
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
