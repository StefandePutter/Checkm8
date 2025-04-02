using UnityEngine;

public class PlayerPawn : PlayerBase
{
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

    public override void Bischop()
    {
        Debug.Log("player bischop");
        if (s_currentBischopCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeBischop();
        // _currentBischopCooldown = _BischopCooldown;
        base.Bischop();
    }

    public override void Rook()
    {
        Debug.Log("player Rook");
        if (s_currentRookCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeRook();
        base.Rook();
    }

    public override void Queen()
    {
        Debug.Log("player Queen");
        if (s_currentQueenCooldown > 0)
        {
            return;
        }

        _gameManager.BecomeQueen();
        base.Queen();
    }

    public override void Horse()
    {
        base.Horse();
    }
}
