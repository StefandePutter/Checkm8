using UnityEngine;

public class PlayerRook : PlayerBase
{
    protected override void Shoot()
    {
        GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.SetActive(true);
        }
    }

    public override void Rook()
    {
        s_currentRookCooldown = _rookCooldown;

        Pawn();
    }

    public override void Horse()
    {
        base.Horse();
    }
}
