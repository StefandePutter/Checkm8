using UnityEngine;

public class PlayerBischop : PlayerBase
{
    [SerializeField] private GameObject _homingPrefab;
    [SerializeField] private float _switchTime=10f;
    private float _maxSwitchTime;
    private int _homingBullets;
    private bool _usedAbility;

    protected override void Start()
    {
        base.Start();
        _maxSwitchTime = _switchTime;
        _gameManager.UiBischopAbility.gameObject.SetActive(true);
    }

    private void Update()
    {
        // if (_usedAbility && _homingBullets == 0)
        if (_homingBullets == 0 && _switchTime <= 0)
        {
            _gameManager.UiQueenAbility.gameObject.SetActive(false);
            s_currentBischopCooldown = _bischopCooldown;
            Pawn();
        }

        _switchTime = Mathf.Max(0,_switchTime);
        _gameManager.UiCharIcon.fillAmount = 1 - _switchTime / _maxSwitchTime;

        _switchTime -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        int rotation = 45;


        for (int i = 0; i < 2; i++)
        {
            if (_homingBullets == 0)
            {
                GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.SetPositionAndRotation(transform.position + Vector3.up * 0.2f, transform.rotation);
                    bullet.transform.Rotate(Vector3.up * rotation);
                    bullet.SetActive(true);
                }
            }
            else
            {
                GameObject bullet = Instantiate(_homingPrefab, transform.position + Vector3.up * 0.2f, transform.rotation);
                bullet.transform.Rotate(Vector3.up * rotation);
                bullet.GetComponent<HomingProjectiles>().SetTarget(FindClosestByTag("Enemy"));
                _homingBullets--;
            }

            rotation *= -1;
        }
    }

    public override void Bischop()
    {
        if (!_usedAbility)
        {
            _homingBullets = 6;
            
            _gameManager.UiBischopAbility.gameObject.SetActive(false);
            
            _usedAbility = true;
        }
    }

    public override void Rook()
    {
        if (s_currentRookCooldown > 0)
        {
            return;
        }

        s_currentBischopCooldown = _bischopCooldown;
        _gameManager.UiBischopAbility.gameObject.SetActive(false);
        base.Rook();
    }

    public override void Queen()
    {
        if (s_currentQueenCooldown > 0)
        {
            return;
        }

        s_currentBischopCooldown = _bischopCooldown;
        _gameManager.UiBischopAbility.gameObject.SetActive(false);
        base.Queen();
    }

    GameObject FindClosestByTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public override void Horse()
    {
        if (s_currentHorseCooldown > 0)
        {
            return;
        }

        s_currentBischopCooldown = _bischopCooldown;
        _gameManager.UiBischopAbility.gameObject.SetActive(true);
        base.Horse();
    }
}
