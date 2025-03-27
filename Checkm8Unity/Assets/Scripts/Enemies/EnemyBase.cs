using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;
    [SerializeField] protected float _score;

    protected bool _moving;

    protected float _timeToFire;
    protected Rigidbody _rb;

    void Awake()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody>();
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

        Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z-amount);
        while (transform.position.z != target.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
            
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        _moving = false;
    }

    protected abstract void Shoot();
}
