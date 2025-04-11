using UnityEngine;

public class EnemyPawn : EnemyBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_allowedToMove)
        {
            // try moving one square down
            StartCoroutine(MoveAmountDown(1));
        }
    }

    protected override void Shoot()
    {
        // get bullet from pool
        GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
            bullet.SetActive(true);
        }
        
        // disable after time passed
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }
}
