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

        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.5f));
    }

    public override void Bischop()
    {
        Debug.Log("player bischop");
        if (_currentBischopCooldown > 0)
        {
            Debug.Log("yeah");
            return;
        }

        _gameManager.BecomeBischop();
        _currentBischopCooldown = _BischopCooldown;
        base.Bischop();
    }

    public override void Horse()
    {
        base.Horse();
    }
}
