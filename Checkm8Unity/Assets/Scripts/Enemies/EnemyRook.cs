using UnityEngine;

public class EnemyRook : EnemyBase
{

    protected void Update()
    {
        //base.FixedUpdate();

        if (!_allowedToMove)
        {
            int[] spawnPosses = new int[11] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 };
            int random = Random.Range(0, spawnPosses.Length);
            int spawnPos = spawnPosses[random];

            StartCoroutine(MoveHorizontal(spawnPos));
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
        // StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }
}
