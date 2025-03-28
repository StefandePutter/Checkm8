using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;

    protected float _horseCooldown = 5f;
    protected float _BischopCooldown = 7.5f;
    protected float _TowerCooldown = 10f;
    protected float _QueenCooldown = 15f;

    protected Collider _playerCollider;
    protected PlayerInputManager _inputManager;
    protected float _timeToFire;
    protected Rigidbody _rb;

    static protected float _currentHorseCooldown;
    static protected float _currentBischopCooldown;
    static protected float _currentTowerCooldown;
    static protected float _currentQueenCooldown;

    protected virtual void Start()
    {
        _playerCollider = GetComponent<Collider>();
        _gameManager = GameManager.s_Instance;
        _rb = GetComponent<Rigidbody>();
        _inputManager = _gameManager.InputManager;
    }

    protected virtual void FixedUpdate()
    {
        _rb.AddForce(new Vector3(_inputManager.MoveValue.x * _moveSpeed, 0, _inputManager.MoveValue.y * _moveSpeed));

        if (_timeToFire <= Time.time)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }

        _gameManager.UiHorse.fillAmount = _currentHorseCooldown / _horseCooldown;
        _gameManager.UiBischop.fillAmount = 1- _currentBischopCooldown / _BischopCooldown;
        _gameManager.UiTower.fillAmount = 1 - _currentTowerCooldown / _TowerCooldown;
        _gameManager.UiQueen.fillAmount = 1- _currentQueenCooldown / _QueenCooldown;

        _currentHorseCooldown -= Time.deltaTime;
        _currentBischopCooldown -= Time.deltaTime;
        _currentTowerCooldown -= Time.deltaTime;
        _currentQueenCooldown -= Time.deltaTime;
    }

    public virtual void Pawn()
    {
        _gameManager.BecomePawn();
    }

    public virtual void Horse()
    {
        if (_currentHorseCooldown > 0)
        {
            return;
        }
        
        _gameManager.BecomeHorse();
        // _currentHorseCooldown = _horseCooldown;
        // PlayerBase._currentBischopCooldown = _BischopCooldown;
        // PlayerBase._currentTowerCooldown = _TowerCooldown;
        // PlayerBase._currentQueenCooldown = _QueenCooldown;
    }

    public virtual void Bischop()
    {
        
    }

    public virtual void Ability()
    {

    }

    public void TakeDamage(float amount=1)
    {
        Debug.Log(name + " took " + amount +" damage");

        _gameManager.Damage();
    }

    protected abstract void Shoot();
}
