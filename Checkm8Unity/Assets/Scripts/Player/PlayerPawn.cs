using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Shoot()
    {
        GameObject bullet = _gameManager.playerBulletsPool.GetPooledObject();
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
        if (_currentHorseCooldown > 0)
        {
            Debug.Log("yeah");
            return;
        }
        base.Horse();
        _gameManager.BecomeHorse();
    }
}
