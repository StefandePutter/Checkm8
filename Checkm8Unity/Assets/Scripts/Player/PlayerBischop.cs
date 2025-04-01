using UnityEngine;

public class PlayerBischop : PlayerBase
{
    [SerializeField] private GameObject _homingPrefab;

    protected override void Shoot()
    {
        int rotation = 45;
        for (int i = 0; i < 2; i++)
        {
            GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.Rotate(Vector3.up * rotation);
                bullet.SetActive(true);
            }
            rotation *= -1;
        }
    }

    public override void Bischop()
    {
        int rotation = 45;
        for (int i = 0; i < 2; i++)
        {
            GameObject bullet = Instantiate(_homingPrefab, transform.position, transform.rotation);
            bullet.transform.Rotate(Vector3.up * rotation);
            rotation *= -1;
        }
        s_currentBischopCooldown = _bischopCooldown;


        //Pawn();
    }

    public override void Horse()
    {
        base.Horse();
    }
}
