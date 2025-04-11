using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;
    private float _size;

    private void Update()
    {
        _size += 1/0.3f * Time.deltaTime;
        _collider.radius = Mathf.Lerp(0, 0.7f, _size);
    }

    private void OnTriggerEnter(Collider other)
    {
        // try damaging collider
        if (other.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(2);
        }
    }
}
