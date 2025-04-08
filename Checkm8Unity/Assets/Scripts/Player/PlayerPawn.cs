using UnityEngine;

public class PlayerPawn : PlayerBase
{
    protected override void Start()
    {
        base.Start();
        _gameManager.UiCharIcon.fillAmount = 0;
    }

    protected override void Shoot()
    {
        GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position + Vector3.up*0.2f, transform.rotation);
            bullet.SetActive(true);
        }

        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.5f));
    }
}
