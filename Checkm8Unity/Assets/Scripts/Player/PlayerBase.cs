using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;

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

    private Vector3 _targetPos;

    protected virtual void Start()
    {
        _playerCollider = GetComponent<Collider>();
        _gameManager = GameManager.s_Instance;
        _rb = GetComponent<Rigidbody>();
        _inputManager = _gameManager.InputManager;
    }

    protected virtual void FixedUpdate()
    {
        if (_timeToFire <= 0)
        {
            _timeToFire = _shootSpeed;
            Shoot();
        }
        _timeToFire -= Time.deltaTime;
        
        _rb.AddForce(new Vector3(_inputManager.MoveValue.x * _moveSpeed, 0, _inputManager.MoveValue.y * _moveSpeed));

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

    }

    public virtual void Rook()
    {

    }

    public virtual void Queen()
    {

    }

    public virtual void Ability()
    {
        // does nothing
    }

    public void TakeDamage(float amount=1)
    {
        // Debug.Log(name + " took " + amount +" damage");

        _gameManager.Damage();
    }

    public static void Reset()
    {
        s_currentHorseCooldown = 0;
        s_currentBischopCooldown = 0;
        s_currentRookCooldown = 0;
        s_currentQueenCooldown = 0;
    }

    protected abstract void Shoot();
}
