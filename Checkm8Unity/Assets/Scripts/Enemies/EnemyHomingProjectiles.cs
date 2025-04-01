using UnityEngine;

public class EnemyHomingProjectiles : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private GameObject _target;
    private Rigidbody _rb;
    private float _lifetime;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_target == null)
        {
            _target = GameManager.s_Player;
        }
        
    }

    void Update()
    {
        if (_lifetime < 0.1f)
        {
            _lifetime += Time.deltaTime;
            _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);
            return;
        }

        if (_target == null)
        {
            _target = _target = GameManager.s_Player;
        }

        Transform target = _target.transform;
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*_rotateSpeed);
        _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position,target.position) < 2)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(0.5f);
        }
        Destroy(gameObject);
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
