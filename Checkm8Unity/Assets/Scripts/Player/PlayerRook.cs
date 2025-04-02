using System.Collections;
using UnityEngine;

public class PlayerRook : PlayerBase
{
    private bool _shootingLaser;

    [SerializeField] private GameObject _laserPrefab;

    protected override void Shoot()
    {
        if (!_shootingLaser)
        {
            GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.SetActive(true);
            }
        }
    }

    public override void Rook()
    {
        _shootingLaser = true;

        StartCoroutine(Laser());

        //laser.transform.localPosition = );
        // Pawn();
    }

    private IEnumerator Laser()
    {
        _inputManager.LockedMovement = true;
        _inputManager.LockedAbilities = true;

        GameObject laser = Instantiate(_laserPrefab, transform);
        laser.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        

        yield return new WaitForSeconds(1f);
        _inputManager.LockedMovement = false;
        _inputManager.LockedAbilities = false;

        s_currentRookCooldown = _rookCooldown;

        Pawn();
    }

    public override void Horse()
    {
        s_currentRookCooldown = _rookCooldown;
        base.Horse();
    }
}
