using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log(name + " Took Damage");
        }
    }

    public void TakeDamage(float amount = 1)
    {
        Debug.Log(name + " took " + amount + " damage");
    }

    protected IEnumerator MoveAmountOfBlocks(int amountOfBlocksDown, int amountOfBlocksRight)
    {
        amountOfBlocksDown *= 2;
        amountOfBlocksRight *= 2;



        yield return null;
    }

    protected abstract void Shoot();
}
