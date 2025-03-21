using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Shoot()
    {
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }

        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }

    public override void Horse()
    {
        _gameManager.BecomeHorse();
        base.Horse();
    }
}
