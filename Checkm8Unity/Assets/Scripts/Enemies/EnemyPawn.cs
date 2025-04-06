using UnityEngine;

public class EnemyPawn : EnemyBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_allowedToMove)
        {
            StartCoroutine(MoveAmountDown(1));
        }
    }

    protected override void Shoot()
    {
        GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
            bullet.SetActive(true);
        }
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }
}
