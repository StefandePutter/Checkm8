using System.Collections;
using UnityEngine;

public class PlayerHorse : PlayerBase
{
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private GameObject _arrowsPrefab;
    [SerializeField] private float _jumpSpeed;

    private GameObject _ghost;
    private GameObject _UiArrows;
    private bool _currentDirRight;

    void Start()
    {
        _currentDirRight = _inputManager.lastMovedRight;
        _UiArrows = Instantiate(_arrowsPrefab, transform);
        _ghost = Instantiate(_ghostPrefab, transform);

        float xPos = -2;
        if (_inputManager.lastMovedRight)
        {
            _UiArrows.transform.Rotate(new Vector3(0, 0, 180));
            xPos *= -1;
        }
        _ghost.transform.localPosition = new Vector3(xPos, 0, 4);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_currentDirRight != _inputManager.lastMovedRight)
        {
            if (_ghost != null)
            {
                FlipArrows();
            }
            _currentDirRight = _inputManager.lastMovedRight;
        }
    }

    public void FlipArrows()
    {
        _UiArrows.transform.Rotate(new Vector3(0, 0, 180));

        // invert the x localPos of the ghost indicator
        _ghost.transform.localPosition = new Vector3(_ghost.transform.localPosition.x*-1, 0, 4);
    }

    public IEnumerator Jump()
    {
        // disable collider and movement/abilities
        _playerCollider.enabled = false;
        _inputManager.lockedMovement = true;
        _inputManager.lockedAbilities = true;

        // get pos to jump and disable ui
        Vector3 jumpPos = _ghost.transform.position;
        Destroy(_ghost);
        Destroy(_UiArrows);


        // first go forward
        Vector3 target;
        target = new Vector3(transform.position.x, transform.position.y, jumpPos.z);
        while(transform.position.z != jumpPos.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed);
            yield return null;
        }

        // then sideways
        target = new Vector3(jumpPos.x,transform.position.y,transform.position.z);
        while(transform.position.x !=  jumpPos.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed);
            yield return null;
        }

        // enable collider and movement/abilities again
        _inputManager.lockedMovement = false;
        _inputManager.lockedAbilities = false;
        _playerCollider.enabled = true;

        // change back into a pawn
        _currentHorseCooldown = _horseCooldown;
        Pawn();
    }

    public override void Horse()
    {
        return;
    }

    protected override void Shoot()
    {
        return;
    }
}
