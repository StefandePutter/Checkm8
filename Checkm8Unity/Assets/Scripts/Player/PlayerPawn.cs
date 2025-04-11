using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Shoot()
    {
        // get bullet out of pool
        GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position + Vector3.up*0.2f, transform.rotation);
            bullet.SetActive(true);
        }

        // disable bullet after time
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.5f));
    }
}
