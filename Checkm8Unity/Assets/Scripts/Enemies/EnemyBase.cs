using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public bool ManuallyActivate; // If true, enemy must be manually activated instead of appearing on screen

    private int _id;
    private static int s_id = 0; // Shared ID counter for all enemies
    private bool _enabled;

    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private GameObject _deathSoundPrefab;

    protected GameManager _gameManager;
    [SerializeField] private GameObject _highlightPrefab;

    [SerializeField] protected float _health = 1;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;
    [SerializeField] protected float _score;
    [SerializeField] private float _waitTime = 0.5f;

    protected int[] _fieldSpacesX;
    protected LayerMask _layerMask;

    protected bool _allowedToMove;
    protected bool _usedAbility;
    protected bool _canFire;
    protected float _timeToFire;

    protected Rigidbody _rb;

    protected virtual void Start()
    {
        _gameManager = GameManager.s_Instance;
        _fieldSpacesX = _gameManager.SpawnPosesX;

        _rb = GetComponent<Rigidbody>();
        _layerMask = LayerMask.GetMask("Enemy", "EnemyHorse", "Environment", "Player");

        // set enemy in GameManager with a unique ID
        _id = s_id;
        s_id++;
        _gameManager.MovePlaces.Add(_id, Vector3.up);
    }

    // enable the enemy
    public virtual IEnumerator EnableEnemy()
    {
        if (!_enabled)
        {
            _enabled = true;

            // highlight the tile when enabling enemy
            Instantiate(_highlightPrefab, transform.position + Vector3.up * 0.02f, transform.rotation);
            
            yield return new WaitForSeconds(0.3f);
            _canFire = true;
            _allowedToMove = true;
        }
    }

    // destroy enemy when it leaves screen
    private void OnBecameInvisible()
    {
        _gameManager.MovePlaces.Remove(_id);
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        // enable when fixed distance from camera
        if (!_enabled && !ManuallyActivate)
        {
            if (Vector3.Distance(transform.position, _gameManager.CameraTransform.position) < 16)
            {
                StartCoroutine(EnableEnemy());
            }
        }

        // Attempt to shoot if allowed
        if (_timeToFire <= Time.time && _canFire)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    // Reduces health and plays feedback
    public virtual void TakeDamage(float amount = 1)
    {
        _health -= amount;
        _hitSound.Play();

        if (_health <= 0)
        {
            Die();
        }
    }

    // Handles enemy death and cleanup
    protected virtual void Die()
    {
        Instantiate(_deathSoundPrefab, transform.position, transform.rotation);
        _gameManager.AddPlayerTime(_score);
        _gameManager.MovePlaces.Remove(_id);
        Destroy(gameObject);
    }

    // slowly move enemy down
    protected IEnumerator MoveAmountDown(int amount)
    {
        _allowedToMove = false;
        amount *= 2;

        float zPos = transform.position.z - amount;
        int directionZ = 2;
        if (amount < 0) directionZ *= -1;

        while (transform.position.z != zPos)
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - directionZ);

            // check it something is on the tile it wants to go
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out RaycastHit hit, 3, _layerMask))
            {
                _allowedToMove = true;
                yield break;
            }
            else
            {
                // check if no one else wants to go to this spot
                if (!CheckTargetPosition(target)) yield break;

                while (transform.position.z != target.z)
                {
                    // move towards spot
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }

            yield return null;
        }

        // punishment time
        yield return new WaitForSeconds(_waitTime);
        _allowedToMove = true;
    }

    // slowly move enemy diagonal to a given X position
    protected IEnumerator MoveAmountDiagonal(int xPos, bool goingUp = false)
    {
        _allowedToMove = false;

        float zPos = xPos - transform.position.x;
        int multiplier = zPos < 0 ? -1 : 1;
        int directionZ = goingUp ? -2 : 2;

        while (transform.position.x != xPos)
        {
            Vector3 target = new Vector3(transform.position.x + (2 * multiplier), transform.position.y, transform.position.z - directionZ);

            // check it something is on the tile it wants to go
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out RaycastHit hit, 3, _layerMask))
            {
                _allowedToMove = true;
                yield break;
            }
            else
            {
                // check if no one else wants to go to this spot
                if (!CheckTargetPosition(target)) yield break;

                while (transform.position.x != target.x)
                {
                    // move towards spot
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }

            yield return null;
        }

        // punishment time
        yield return new WaitForSeconds(_waitTime);
        _allowedToMove = true;
    }

    // slowly move enemy horizontal to given xPos
    protected IEnumerator MoveHorizontal(int xPos)
    {
        _allowedToMove = false;

        float zPos = xPos - transform.position.x;
        int multiplier = zPos < 0 ? -1 : 1;

        while (transform.position.x != xPos)
        {
            Vector3 target = new Vector3(transform.position.x + (2 * multiplier), transform.position.y, transform.position.z);

            // check it something is on the tile it wants to go
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out RaycastHit hit, 3, _layerMask))
            {
                _allowedToMove = true;
                yield break;
            }
            else
            {
                // check if no one else wants to go to this spot
                if (!CheckTargetPosition(target)) yield break;

                while (transform.position.x != target.x)
                {
                    // move towards spot
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }

            yield return null;
        }

        // punishment time
        yield return new WaitForSeconds(_waitTime);
        _allowedToMove = true;
    }

    // Checks whether the given move target is already taken
    private bool CheckTargetPosition(Vector3 target)
    {
        if (!_gameManager.MovePlaces.ContainsValue(target))
        {
            _gameManager.MovePlaces[_id] = target;
            return true;
        }
        else
        {
            _allowedToMove = true;
            return false;
        }
    }

    protected abstract void Shoot();
}
