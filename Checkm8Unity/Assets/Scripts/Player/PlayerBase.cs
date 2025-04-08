using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;

    static protected Vector3 s_moveTarget = Vector3.zero;

    protected float _horseCooldown = 5f;
    protected float _bischopCooldown = 7.5f;
    protected float _rookCooldown = 10f;
    protected float _queenCooldown = 15f;

    protected Collider _playerCollider;
    protected PlayerInputManager _inputManager;
    protected float _timeToFire;
    protected Rigidbody _rb;

    static protected float s_currentHorseCooldown;
    static protected float s_currentBischopCooldown;
    static protected float s_currentRookCooldown;
    static protected float s_currentQueenCooldown;

    private float _damageDelay = 0.5f;
    private float _damageCooldown;
    protected bool _allowedMovement = true;
    private LayerMask _raycastLayerMask;
    protected bool _canFindMove = true;

    protected virtual void Start()
    {
        _playerCollider = GetComponent<Collider>();
        _gameManager = GameManager.s_Instance;
        _rb = GetComponent<Rigidbody>();
        
        _raycastLayerMask = LayerMask.GetMask("Enemy", "EnemyHorse", "Environment");

        _inputManager = _gameManager.InputManager;

        _gameManager.UiCharIcon.fillAmount = 0;
    }

    protected virtual void FixedUpdate()
    {
        if (_timeToFire <= 0)
        {
            _timeToFire = _shootSpeed;
            Shoot();
        }
        _timeToFire -= Time.deltaTime;

        if (_damageCooldown > 0)
        {
            _damageCooldown -= Time.deltaTime;
        }

        if (_inputManager.MoveValue != Vector2.zero && _canFindMove)
        {
            Vector3 target = s_moveTarget + new Vector3(_inputManager.MoveValue.x, 0, _inputManager.MoveValue.y) * 2;

            if (target.z <= _gameManager.CameraTransform.position.z-1.165 && _inputManager.MoveValue.y == -1)
            {
                return;
            }

            if (target.z >= _gameManager.CameraTransform.position.z + 14 && _inputManager.MoveValue.y == 1)
            {
                return;
            }

            RaycastHit hit;
            if (!Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 3, _raycastLayerMask, QueryTriggerInteraction.Ignore))
            {
                s_moveTarget = target;
                _canFindMove = false;
            }
        }
        
        if (_allowedMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, s_moveTarget, Time.deltaTime * _moveSpeed);
            if (s_moveTarget == transform.position)
            {
                _canFindMove = true;
            }
        }

        _gameManager.UiHorse.fillAmount = s_currentHorseCooldown / _horseCooldown;
        _gameManager.UiBischop.fillAmount = s_currentBischopCooldown / _bischopCooldown;
        _gameManager.UiTower.fillAmount = s_currentRookCooldown / _rookCooldown;
        _gameManager.UiQueen.fillAmount = s_currentQueenCooldown / _queenCooldown;

        s_currentHorseCooldown -= Time.deltaTime;
        s_currentBischopCooldown -= Time.deltaTime;
        s_currentRookCooldown -= Time.deltaTime;
        s_currentQueenCooldown -= Time.deltaTime;
    }

    public virtual void Pawn()
    {
        _gameManager.BecomePawn();
    }

    public virtual void Horse()
    {
        if (s_currentHorseCooldown > 0)
        {
            return;
        }
        
        _gameManager.BecomeHorse();
    }

    public virtual void Bischop()
    {
        Debug.Log("player bischop");
        if (s_currentBischopCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeBischop();
    }

    public virtual void Rook()
    {
        Debug.Log("player Rook");
        if (s_currentRookCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeRook();
    }

    public virtual void Queen()
    {
        Debug.Log("player Queen");
        if (s_currentQueenCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeQueen();
    }

    public void TakeDamage(float amount=1)
    {
        // Debug.Log(name + " took " + amount +" damage");

        if (_damageCooldown > 0)
        {
            return;
        }

        _damageCooldown = _damageDelay;

        _gameManager.Damage();
    }

    public void SetTarget(Vector3 target)
    {
        s_moveTarget = target;
    }

    public static void Reset()
    {
        s_moveTarget = Vector3.zero;

        s_currentHorseCooldown = 0;
        s_currentBischopCooldown = 0;
        s_currentRookCooldown = 0;
        s_currentQueenCooldown = 0;
    }

    protected abstract void Shoot();
}
