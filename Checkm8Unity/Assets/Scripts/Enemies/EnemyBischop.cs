using UnityEngine;

public class EnemyBischop : EnemyBase
{
    [SerializeField] private GameObject _homingPrefab;
    private int _homingBullets;
    private int _timesMoved;

    protected void Update()
    {
        if (!_usedAbility)
        {
            if (_timesMoved > 0 && _allowedToMove)
            {
                _homingBullets=2;
                _usedAbility = true;
            }
        }

        if (_allowedToMove)
        {
            int[] spawnPosses = _fieldSpacesX; // new int[7] { -6, -4, -2, 0, 2, 4, 6 };
            int random = Random.Range(0, spawnPosses.Length);
            int spawnPos = spawnPosses[random];

            StartCoroutine(MoveAmountDiagonal(spawnPos));
            _timesMoved++;
        }
    }

    protected override void Shoot()
    {
        int rotation = 45;
        for (int i =0; i < 2; i++)
        {
            if (_homingBullets == 0)
            {
                GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                    bullet.transform.Rotate(Vector3.up * rotation);
                    bullet.SetActive(true);
                }
            }
            else
            {
                GameObject bullet = Instantiate(_homingPrefab, transform.position, transform.rotation);
                bullet.transform.Rotate(Vector3.up * rotation);
                _homingBullets--;
            }
            rotation *= -1;
        }
    }
}
