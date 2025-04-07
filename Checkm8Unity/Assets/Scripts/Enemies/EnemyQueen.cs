using System.Collections;
using UnityEngine;

public class EnemyQueen : EnemyBase
{
    [SerializeField] LayerMask _nukeLayerMask;
    [SerializeField] GameObject _nukeIndicatorPrefab;
    private float _maxHealth;

    protected override void Start()
    {
        base.Start();

        _maxHealth = _health;
    }

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
        if (_health <= _maxHealth / 3 && !_usedAbility && _allowedToMove)
        {
            Debug.Log("nuke");
            StartCoroutine(StartNuke());
            _usedAbility = true;
        }

        if (_allowedToMove)
        {
            int randomInt = Random.Range(0,3);
            int min = -(int)(304 - transform.position.z) / 2;
            int max = (int)(transform.position.z - 294) / 2;
            int random;
            int spawnPos;

            switch (randomInt)
            {
                case 0:
                    bool picking = true;
                    spawnPos = 0;
                    int i = 0;
                    bool up = false;
                    while (picking)
                    {
                        random = Random.Range(0, _fieldSpacesX.Length);
                        spawnPos = _fieldSpacesX[random];
                        float target = (transform.position.x - spawnPos) /2;
                        if (target < 0)
                            target *= -1;
                        if (Random.Range(0,2) == 0)
                        {
                            up = true;
                            
                            if (target <= -min)
                            {
                                picking = false;
                            }

                            // to keep from crashing if something goes wrong
                            if (i == 20)
                            {
                                Debug.Log("failed: " + transform.position.x + ". " + spawnPos + ", " + min + ", " + max + ", " + (transform.position.x - spawnPos));
                                spawnPos= 100;
                                picking = false;
                            }
                            i++;
                        }
                        else
                        {
                            up = false;

                            if (target <= max)
                            {
                                picking = false;
                            }

                            // to keep from crashing if something goes wrong
                            if (i == 20)
                            {
                                Debug.Log("failed: " + transform.position.x + ". " + spawnPos + ", " + min + ", " + max + ", " + (transform.position.x - spawnPos));
                                spawnPos = 100;
                                picking = false;
                            }
                            i++;
                        }
                    }
                    if (spawnPos != 100)
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
        }
    }

    private IEnumerator StartNuke()
    {
        _allowedToMove = false;
        _canFire = false;
        GameObject indicator = Instantiate(_nukeIndicatorPrefab, transform.position+Vector3.up*0.2f, transform.rotation);
        yield return new WaitForSeconds(1f);
        // indicator.SetActive(false);
        Destroy(indicator);
        Nuke();
        _allowedToMove = true;
        _canFire = true;
    }

    private void Nuke()
    {
        Collider[] Hits = new Collider[25];

        int hits = Physics.OverlapSphereNonAlloc(transform.position, 25, Hits, _nukeLayerMask);

        _nukeLayerMask += LayerMask.GetMask("ProjectilePlayer", "ProjectileEnemy");

        for (int i = 0; i < hits; i++)
        {
            //if (Hits[i].TryGetComponent)
            float distance = Vector3.Distance(transform.position, Hits[i].transform.position);

            Debug.Log($"Queen would hit {Hits[i].name} for {1}");
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
