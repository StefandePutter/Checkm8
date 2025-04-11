using System.Collections;
using UnityEngine;

public class EnemyRook : EnemyBase
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _indicatorPrefab;
    private bool _isShootingLaser;
    private int _amountMoved;

    protected void Update()
    {
        if (!_usedAbility && _allowedToMove)
        {
            _usedAbility = true;
            
            // shoot laser
            StartCoroutine(Laser());
        }

        if (_allowedToMove)
        {
            int random = Random.Range(0, _fieldSpacesX.Length);
            int spawnPos = _fieldSpacesX[random];

            StartCoroutine(MoveHorizontal(spawnPos));
            _amountMoved++;
        }
    }

    protected override void Shoot()
    {
        if (!_isShootingLaser)
        {
            GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.SetActive(true);
            }
        }
    }

    private IEnumerator Laser()
    {
        _isShootingLaser = true;
        _canFire = false;
        _allowedToMove = false;

        GameObject indicator = Instantiate(_indicatorPrefab, transform);
        indicator.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        // showing indicator
        yield return new WaitForSeconds(0.5f);
        indicator.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        indicator.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Destroy(indicator);
        yield return new WaitForSeconds(0.2f);

        // shoot laser
        GameObject laser = Instantiate(_laserPrefab, transform);
        laser.GetComponent<Laser>().damage = 3;
        laser.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        // shoot for a second
        yield return new WaitForSeconds(1f);

        Destroy(laser);

        // punish time
        yield return new WaitForSeconds(0.5f);

        _isShootingLaser = false;
        _canFire = true;
        _allowedToMove = true;
    }
}
