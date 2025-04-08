using System.Collections;
using UnityEngine;

public class EnemyQueen : EnemyBase
{
    [SerializeField] LayerMask _nukeLayerMask;
    [SerializeField] GameObject _nukeIndicatorPrefab;
    [SerializeField] private float _maxHealth = 300f;
    [HideInInspector] public bool IsBoss = true;
    private float _startPosZ;

    protected override void Start()
    {
        base.Start();

        if (IsBoss)
        {
            _gameManager.ToggleBossBattle(_maxHealth);
        }
        else
        {
            _maxHealth = _health;
        }


        _startPosZ = transform.position.z;
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

    public override void TakeDamage(float amount = 1)
    {
        if (IsBoss)
        {
            amount *= 10;

            _gameManager.RemoveEnemyTime(amount);

            if (_gameManager.TimeEnemy <= 0)
            {
                _gameManager.ToggleBossBattle();
                Die();
            }
        }
        else
        {
            base.TakeDamage(amount);
        }
    }

    private void Update()
    {
        if (_gameManager.TimeEnemy <= 0)
        {
            _gameManager.ToggleBossBattle();
            Die();
        }

        // do ability when close to death
        float health = (IsBoss) ? _gameManager.TimeEnemy : _health;
        if (health <= _maxHealth / 3 && !_usedAbility && _allowedToMove)
        {
            Debug.Log("nuke");
            StartCoroutine(StartNuke());
            _usedAbility = true;
        }

        // when allowed to move
        if (_allowedToMove)
        {
            // set min and max board pos it may go
            int min = -(int)((_startPosZ + 4) - transform.position.z) / 2;
            int max = (int)(transform.position.z - (_startPosZ - 6)) / 2;
            
            // choose random move option
            int randomInt = Random.Range(0,3);
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

                        // randomly pick up or down
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

    protected override void Die()
    {
        // start cam again
        if (IsBoss)
        {
            Camera.main.GetComponent<CameraMovement>().ResetTarget();
        }

        base.Die();
    }
}
