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

        // if no target set default to player
        if (_target == null)
        {
            _target = GameManager.s_Player;
        }
        
    }

    void Update()
    {
        // fly normally before homing
        if (_lifetime < 0.1f)
        {
            _lifetime += Time.deltaTime;
            _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);
            return;
        }

        // if target died
        if (_target == null)
        {
            _target = _target = GameManager.s_Player;
        }

        // Rotate towards the target
        Transform target = _target.transform;
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*_rotateSpeed);
        _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);

        // destroy when close to player
        if (Vector3.Distance(transform.position,target.position) < 2)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // try damaging the enemy
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
        {
            component.TakeDamage(0.5f);
        }
        Destroy(gameObject);
    }

    // set target
    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
