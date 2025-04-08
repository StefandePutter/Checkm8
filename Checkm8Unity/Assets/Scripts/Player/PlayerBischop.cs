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
    }

    private void Update()
    {
        // if (_homingBullets == 0 && _switchTime <= 0)
        if (_usedAbility && _homingBullets == 0)
        {
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
                GameObject bullet = Instantiate(_homingPrefab, transform.position, transform.rotation);
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
        }

        _usedAbility = true;



        //Pawn();
    }

    public override void Rook()
    {
        s_currentBischopCooldown = _bischopCooldown;
        base.Rook();
    }

    public override void Queen()
    {
        s_currentBischopCooldown = _bischopCooldown;
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
        s_currentBischopCooldown = _bischopCooldown;
        base.Horse();
    }
}
