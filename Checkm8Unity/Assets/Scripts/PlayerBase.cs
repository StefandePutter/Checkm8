using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;

    protected Collider _playerCollider;
    protected PlayerInputManager _inputManager;
    protected float _timeToFire;
    protected Rigidbody _rb;

    void Awake()
    {
        _playerCollider = GetComponent<Collider>();
        _gameManager = FindFirstObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody>();
        _inputManager = _gameManager.GetComponent<PlayerInputManager>();
    }

    protected virtual void FixedUpdate()
    {
        _rb.AddForce(new Vector3(_inputManager.MoveValue.x * _moveSpeed, 0, _inputManager.MoveValue.y * _moveSpeed));

        if (_timeToFire <= Time.time)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    public virtual void Pawn()
    {
        _gameManager.BecomePawn();
    }

    public virtual void Horse()
    {
        return;
    }

    protected abstract void Shoot();
}
