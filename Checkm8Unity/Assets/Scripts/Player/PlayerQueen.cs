using System.Collections;
using UnityEngine;

public class PlayerQueen : PlayerBase
{
    // [SerializeField] LayerMask _nukeLayerMask;
    [SerializeField] private float _switchTime = 10f;
    [SerializeField] private GameObject _nukeParticlePrefab;
    private float _maxSwitchTime;
    private bool _usingAbility;
    private bool _usedAbility;

    protected override void Start()
    {
        base.Start();
        _maxSwitchTime = _switchTime;
        _gameManager.UiQueenAbility.gameObject.SetActive(true);

    }

    private void Update()
    {
        // if (_usedAbility && _homingBullets == 0)
        if (!_usingAbility && _switchTime <= 0)
        {
            _gameManager.UiQueenAbility.gameObject.SetActive(false);
            s_currentQueenCooldown = _queenCooldown;
            Pawn();
        }

        _switchTime = Mathf.Max(0, _switchTime);
        _gameManager.UiCharIcon.fillAmount = 1 - _switchTime / _maxSwitchTime;

        _switchTime -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        if (!_usingAbility)
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
    }

    public override void Bischop()
    {
        if (s_currentBischopCooldown > 0)
        {
            return;
        }

        s_currentQueenCooldown = _queenCooldown;
        _gameManager.UiQueenAbility.gameObject.SetActive(false);
        base.Bischop();
    }

    public override void Rook()
    {
        if (s_currentRookCooldown > 0)
        {
            return;
        }

        s_currentQueenCooldown = _queenCooldown;
        _gameManager.UiQueenAbility.gameObject.SetActive(false);

        base.Rook();
    }

    public override void Queen()
    {
        if (!_usedAbility)
        {
            _usedAbility = true;

            StartCoroutine(StartNuke());
        }
    }

    private IEnumerator StartNuke()
    {
        _usingAbility = true;

        GameObject particle = Instantiate(_nukeParticlePrefab, transform);
        yield return new WaitForSeconds(7.5f);
        Nuke();
        // Destroy(particle);

        _usingAbility = false;
    }

    private void Nuke()
    {
        Collider[] Hits = new Collider[100];

        Vector3 startPos = transform.position + Vector3.up * 0.2f;

        LayerMask _nukeLayerMask = LayerMask.GetMask("Enemy", "ProjectileEnemy", "EnemyHorse");

        int hits = Physics.OverlapSphereNonAlloc(startPos, 25, Hits, _nukeLayerMask);

        for (int i = 0; i < hits; i++)
        {
            if (Hits[i].CompareTag("Bullet"))
            {
                Hits[i].attachedRigidbody.gameObject.SetActive(false);
            }
            else if (Hits[i].TryGetComponent<IDamageable>(out IDamageable component))
            {
                component.TakeDamage(4);
            }
        }

        _gameManager.UiQueenAbility.gameObject.SetActive(false);
    }

    public override void Horse()
    {
        if (s_currentHorseCooldown > 0)
        {
            return;
        }

        s_currentQueenCooldown = _queenCooldown;
        _gameManager.UiQueenAbility.gameObject.SetActive(false);
        base.Horse();
    }
}
