using UnityEngine;

public class EnemyQueen : EnemyBase
{
    protected override void Shoot()
    {
        int rotation = 0;
        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.transform.Rotate(Vector3.up * rotation);
                bullet.SetActive(true);
            }
            rotation += 45;
        }
    }

    private void Update()
    {
        if (_allowedToMove)
        {
            int randomInt = Random.Range(0,3);
            // Debug.Log(randomInt);
            if (transform.position.z == 304)
            {
                randomInt = Random.Range(0,2);
            }
            else if (transform.position.z == 294)
            {
                randomInt = 2;
            }
            randomInt = 1;
            switch (randomInt)
            {
                case 0:
                    StartCoroutine(MoveAmountDiagonal((int)transform.position.z - 1));
                    break;
                case 1:
                    int random = Random.Range(0, _fieldSpacesX.Length);
                    int spawnPos = _fieldSpacesX[random];

                    StartCoroutine(MoveHorizontal(spawnPos));
                    break;
                case 2:
                    int min = -(int)(304 - transform.position.z) / 2;
                    int max = (int)(294 - transform.position.z / 2);
                    StartCoroutine(MoveAmountDown(Random.Range(min, max)));
                    break;
            }
            // StartCoroutine(MoveAmountDown(1));
        }
    }

    protected override void FixedUpdate()
    {
        // calls shoot function that will be made in child files
        if (_timeToFire <= Time.time && _canFire)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    protected override void Die()
    {
        // start cam again
        Camera.main.GetComponent<CameraMovement>().ResetTarget();

        base.Die();
    }
}
