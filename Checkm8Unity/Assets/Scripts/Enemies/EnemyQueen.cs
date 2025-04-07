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
            int min = -(int)(304 - transform.position.z) / 2;
            int max = (int)(transform.position.z - 294) / 2;
            int random;
            int spawnPos;

            randomInt = 0;
            switch (randomInt)
            {
                case 0:
                    bool picking = true;
                    spawnPos = 0;
                    bool up = false;
                    int i = 0;
                    while (picking)
                    {
                        random = Random.Range(0, _fieldSpacesX.Length);
                        spawnPos = _fieldSpacesX[random];
                        float target = (transform.position.x - spawnPos) /2;
                        if (target < 0)
                            target *= -1;
                        if (Random.Range(0,1) == 0)
                        {
                            up = true;
                            
                            if (target <= -min)
                            {
                                Debug.Log("up: " + transform.position.x + ". " + spawnPos + ", " + -min + ", " + max + ", " + target);
                                picking = false;
                            }

                            if (i == 20)
                            {
                                Debug.Log("failed: " + transform.position.x + ". " + spawnPos + ", " + min + ", " + max + ", " + (transform.position.x - spawnPos));

                                picking = false;
                            }
                            i++;
                        }
                        else
                        {
                            if (target <= max)
                            {
                                Debug.Log("down: " + spawnPos + ", " + min + ", " + max + ", " + (transform.position.x - spawnPos));
                                picking = false;
                            }

                            if (i == 20)
                            {
                                Debug.Log("failed: " + transform.position.x + ". " + spawnPos + ", " + min + ", " + max + ", " + (transform.position.x - spawnPos));

                                picking = false;
                            }
                            i++;
                        }
                    }

                    StartCoroutine(MoveAmountDiagonal(spawnPos, up));
                    break;
                case 1:
                    random = Random.Range(0, _fieldSpacesX.Length);
                    spawnPos = _fieldSpacesX[random];
                    StartCoroutine(MoveHorizontal(spawnPos));
                    break;
                case 2:
                    StartCoroutine(MoveAmountDown(Random.Range(min, max+1)));
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
