using UnityEngine;

public class PlayerQueen : PlayerBase
{
    protected override void Shoot()
    {
        int rotation = 0;
        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.transform.Rotate(Vector3.up * rotation);
                bullet.SetActive(true);
            }   
            rotation += 45;
        }
    }

    public override void Horse()
    {
        s_currentQueenCooldown = _queenCooldown;
        base.Horse();
    }
}
