using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class PlayerRook : PlayerBase
{
    private bool _isShootingLaser;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _switchTime = 10f;
    private float _maxSwitchTime;
    private bool _usingAbility;
    private bool _usedAbility;

    protected override void Start()
    {
        base.Start();
        _maxSwitchTime = _switchTime;

        // show ability icon
        _gameManager.UiTowerAbility.gameObject.SetActive(true);
    }

    private void Update()
    {
        // switch back to pawn when timer is done and not shooting laser
        if (!_isShootingLaser && _switchTime <= 0)
        {
            // deactivate ability icon
            _gameManager.UiTowerAbility.gameObject.SetActive(false);
            
            s_currentRookCooldown = _rookCooldown;
            
            Pawn();
        }

        _switchTime = Mathf.Max(0, _switchTime);
        _gameManager.UiCharIcon.fillAmount = 1 - _switchTime / _maxSwitchTime;

        _switchTime -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        if (!_isShootingLaser)
        {
            // get bullet from pool
            GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.SetActive(true);
            }
        }
    }

    // try changing into Bischop
    public override void Bischop()
    {
        if (s_currentBischopCooldown > 0)
        {
            return;
        }

        // deactivate ability icon
        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        s_currentRookCooldown = _rookCooldown;
        base.Bischop();
    }

    // try using rook ability
    public override void Rook()
    {
        if (!_usedAbility)
        {
            _usedAbility = true;

            _isShootingLaser = true;

            // shoot laser
            StartCoroutine(Laser());

            // deactivate ability icon
            _gameManager.UiTowerAbility.gameObject.SetActive(false);
        }
    }

    public override void Queen()
    {
        if (s_currentQueenCooldown > 0)
        {
            return;
        }

        // deactivate ability icon
        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        // set cooldown
        s_currentRookCooldown = _rookCooldown;

        base.Queen();
    }
    

    private IEnumerator Laser()
    {
        // lock movement
        _inputManager.LockedMovement = true;
        _inputManager.LockedAbilities = true;
        _allowedMovement = false;

        // shoot laser
        GameObject laser = Instantiate(_laserPrefab, transform);
        laser.GetComponent<Laser>().damage = 3;
        laser.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        // for 1 sec
        yield return new WaitForSeconds(1f);
        
        // destroy laser
        Destroy(laser);

        // move again
        _isShootingLaser = false;
        _inputManager.LockedMovement = false;
        _inputManager.LockedAbilities = false;
        _allowedMovement = true;
    }

    // try changing into Horse
    public override void Horse()
    {
        s_currentRookCooldown = _rookCooldown;

        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        base.Horse();
    }
}
