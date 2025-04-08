using System.Collections;
using UnityEngine;

public class PlayerRook : PlayerBase
{
    private bool _isShootingLaser;

    [SerializeField] private GameObject _laserPrefab;

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
        s_currentRookCooldown = _rookCooldown;
        base.Bischop();
    }

    public override void Rook()
    {
        _isShootingLaser = true;

        StartCoroutine(Laser());
    }

    public override void Queen()
    {
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
        _inputManager.LockedMovement = false;
        _inputManager.LockedAbilities = false;
        _allowedMovement = true;

        s_currentRookCooldown = _rookCooldown;

        Pawn();
    }

    public override void Horse()
    {
        s_currentRookCooldown = _rookCooldown;
        base.Horse();
    }
}
