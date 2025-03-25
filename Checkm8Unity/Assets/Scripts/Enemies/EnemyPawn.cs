using UnityEngine;

public class EnemyPawn : EnemyBase, IDamageable
{
    protected override void Shoot()
    {
        GameObject bullet = _gameManager.s_enemyBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }

    public override void TakeDamage(float amount)
    {
        Destroy(gameObject);
    }
}
