using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private GameManager _gameManager;
    private GameObject _target;
    private Rigidbody _rb;
    private float _lifetime;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _target = FindClosestByTag("Enemy");
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
            _target = FindClosestByTag("Enemy");
            if (_target == null)
            {
                _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);
                return;
            }
        }

        Transform target = _target.transform;
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*_rotateSpeed);
        _rb.MovePosition(_rb.position + transform.forward * _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position,target.position) < 0.5f)
        {
            Destroy(gameObject,0.5f);
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

    GameObject FindClosestByTag(string tag)
    {
        // find all gameobjects i know its slow but i didnt know another way
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        // start with inf distance and check if something is closer everytime and then return that
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
