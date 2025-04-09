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

        _gameManager.UiTowerAbility.gameObject.SetActive(true);
    }

    private void Update()
    {
        // if (_usedAbility && _homingBullets == 0)
        if (!_isShootingLaser && _switchTime <= 0)
        {
            _gameManager.UiQueenAbility.gameObject.SetActive(false);
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
            GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.SetActive(true);
            }
        }
    }
    public override void Bischop()
    {
        if (s_currentBischopCooldown > 0)
        {
            return;
        }

        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        s_currentRookCooldown = _rookCooldown;
        base.Bischop();
    }

    public override void Rook()
    {
        if (!_usedAbility)
        {
            _usedAbility = true;

            _isShootingLaser = true;

            StartCoroutine(Laser());

            _gameManager.UiTowerAbility.gameObject.SetActive(false);
        }
    }

    public override void Queen()
    {
        if (s_currentQueenCooldown > 0)
        {
            return;
        }

        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        s_currentRookCooldown = _rookCooldown;

        base.Queen();
    }
    

    private IEnumerator Laser()
    {
        _inputManager.LockedMovement = true;
        _inputManager.LockedAbilities = true;
        _allowedMovement = false;

        GameObject laser = Instantiate(_laserPrefab, transform);
        laser.GetComponent<Laser>().damage = 3;
        // set the local pos
        laser.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        yield return new WaitForSeconds(1f);
        Destroy(laser);

        _isShootingLaser = false;
        _inputManager.LockedMovement = false;
        _inputManager.LockedAbilities = false;
        _allowedMovement = true;
    }

    public override void Horse()
    {
        s_currentRookCooldown = _rookCooldown;

        _gameManager.UiTowerAbility.gameObject.SetActive(false);

        base.Horse();
    }
}
