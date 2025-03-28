using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;
    [SerializeField] protected float _score;
    [SerializeField] private float _waitTime = 0.5f;
    protected LayerMask _layerMask;


    protected bool _moving;

    protected float _timeToFire;
    protected Rigidbody _rb;

    void Awake()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody>();
        _layerMask = LayerMask.GetMask("Enemy", "Environment");
    }

    private void Start()
    {     
        _rb.AddForce(Vector3.back * _moveSpeed);
    }

    protected virtual void FixedUpdate()
    {
        if (_timeToFire <= Time.time)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    public virtual void TakeDamage(float amount = 1)
    {
        Debug.Log(name + " took " + amount + " damage");
        _gameManager.AddPlayerTime(_score);
        Destroy(gameObject);
    }

    protected IEnumerator MoveAmountDown(int amount)
    {
        _moving = true;
        amount *= 2;

        float zPos = transform.position.z - amount;
        while (transform.position.z != zPos)
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

            RaycastHit hit;
            if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);
                _moving = false;
                yield break;
            }
            else
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

                while (transform.position.z != target.z)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
            
            yield return null;
        }

        yield return new WaitForSeconds(_waitTime);

        _moving = false;
    }

    protected IEnumerator MoveAmountDiagonal(int xPos)
    {
        _moving = true;

        float zPos = xPos - transform.position.x;
        int multiplier = 1;
        if (zPos < 0)
        {
            multiplier = -1;
            zPos *= -1;
        }

        Vector3 target = new Vector3(xPos, transform.position.y, transform.position.z - zPos);
        while (transform.position.x != xPos)
        {
            target = new Vector3(transform.position.x+(2*multiplier), transform.position.y, transform.position.z - 2);
            
            RaycastHit hit; 
            if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);
                _moving = false;
                yield break;
            } else
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

                while (transform.position.x != target.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }
            


            yield return null;
        }

        yield return new WaitForSeconds(_waitTime);

        _moving = false;
    }

    protected abstract void Shoot();
}
