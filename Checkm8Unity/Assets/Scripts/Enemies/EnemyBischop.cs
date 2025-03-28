using UnityEngine;

public class EnemyBischop : EnemyBase
{
    protected void Update()
    {
        //base.FixedUpdate();

        if (!_allowedToMove)
        {
            int[] spawnPosses = new int[11] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 };
            int random = Random.Range(0, spawnPosses.Length);
            int spawnPos = spawnPosses[random];


            StartCoroutine(MoveAmountDiagonal(spawnPos));
        }
    }

    protected override void Shoot()
    {
        int rotation = 45;
        for (int i =0; i < 2; i++)
        {
            GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.Rotate(Vector3.up * rotation);
                bullet.SetActive(true);
            }
            rotation *= -1;
        }
        //StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
}
