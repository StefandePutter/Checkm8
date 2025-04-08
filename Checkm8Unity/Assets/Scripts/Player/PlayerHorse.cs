using System.Collections;
using UnityEngine;

public class PlayerHorse : PlayerBase
{
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private GameObject _arrowsPrefab;
    [SerializeField] private GameObject _shockwavePrefab;
    [SerializeField] private float _jumpSpeed;

    private GameObject _ghost;
    private GameObject _UiArrows;
    private bool _currentDirRight;

    protected override void Start()
    {
        base.Start();
        _currentDirRight = _inputManager.LastMovedRight;
        _UiArrows = Instantiate(_arrowsPrefab, transform);
        _ghost = Instantiate(_ghostPrefab, transform);

        float xPos = -2;
        //_UiArrows.transform.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z);
        if (_inputManager.LastMovedRight)
        {
            _UiArrows.transform.Rotate(new Vector3(0, 0, 180));
            xPos *= -1;
        }
        _ghost.transform.localPosition = new Vector3(xPos, 0, 4);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_currentDirRight != _inputManager.LastMovedRight)
        {
            if (_ghost != null)
            {
                FlipArrows();
            }
            _currentDirRight = _inputManager.LastMovedRight;
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
        Vector3 jumpPos = _ghost.transform.position;

        LayerMask layerMask = LayerMask.GetMask("Environment");
        bool notAllowedToJump = true;
        while (notAllowedToJump)
        {
            jumpPos = _ghost.transform.position;

            RaycastHit hit;
            if (!Physics.Raycast(jumpPos - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 3, layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("yes");
                notAllowedToJump = false;
            }


            yield return null;
        }

        // disable collider and movement/abilities
        _playerCollider.enabled = false;
        _inputManager.LockedMovement = true;
        _inputManager.LockedAbilities = true;
        _allowedMovement = false;

        // get pos to jump and disable ui
        Destroy(_ghost);
        Destroy(_UiArrows);


        // first go forward
        Vector3 target;
        target = new Vector3(transform.position.x, transform.position.y+4, transform.position.z);
        while (transform.position.y != target.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed);
            yield return null;
        }

        target = new Vector3(transform.position.x, transform.position.y, jumpPos.z);
        while(transform.position.z != target.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed);
            yield return null;
        }

        // then sideways
        target = new Vector3(jumpPos.x,transform.position.y,transform.position.z);
        while(transform.position.x != target.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed);
            yield return null;
        }

        // then down
        target = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
        while (transform.position.y != target.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _jumpSpeed*3);
            yield return null;
        }

        Instantiate(_shockwavePrefab, transform.position + Vector3.up * 0.2f, transform.rotation);

        // enable collider and movement/abilities again
        _inputManager.LockedMovement = false;
        _inputManager.LockedAbilities = false;
        _playerCollider.enabled = true;

        s_moveTarget = transform.position;
        _allowedMovement = false;

        // change back into a pawn
        s_currentHorseCooldown = _horseCooldown;
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
