using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyRook : EnemyBase
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _indicatorPrefab;
    private bool _isShootingLaser;
    private int _amountMoved;

    protected void Update()
    {
        //base.FixedUpdate();

        if (_amountMoved > 1 && !_usedAbility && _allowedToMove)
        {
            _usedAbility = true;
            // _allowedToMove = false;
            StartCoroutine(Laser());
        }

        if (_allowedToMove)
        {
            int[] spawnPosses = _fieldSpacesX;
            int random = Random.Range(0, spawnPosses.Length);
            int spawnPos = spawnPosses[random];

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
        yield return new WaitForSeconds(1.5f);

        Destroy(indicator);
        yield return null;

        GameObject laser = Instantiate(_laserPrefab, transform);
        laser.GetComponent<Laser>().damage = 3;
        // set the local pos
        laser.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);

        // shoot for a second
        yield return new WaitForSeconds(1f);

        Destroy(laser);

        yield return new WaitForSeconds(0.5f);


        _isShootingLaser = false;
        _canFire = true;
        _allowedToMove = true;
    }
}
