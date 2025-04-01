using UnityEngine;

public class PlayerBischop : PlayerBase
{
    [SerializeField] private GameObject _homingPrefab;
    private int _homingBullets;
    private bool _usedAbility;

    private void Update()
    {
        if (_usedAbility && _homingBullets==0)
        {
            s_currentBischopCooldown = _bischopCooldown;
            Pawn();
        }
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
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = transform.rotation;
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
            //GameObject bullet = _gameManager.PlayerBulletsPool.GetPooledObject();
            //if (bullet != null)
            //{
            //    bullet.transform.position = transform.position;
            //    bullet.transform.rotation = transform.rotation;
            //    bullet.transform.Rotate(Vector3.up * rotation);
            //    bullet.SetActive(true);
            //}
            rotation *= -1;
        }
    }

    public override void Bischop()
    {
        //int rotation = 45;
        //for (int i = 0; i < 2; i++)
        //{
        //    GameObject bullet = Instantiate(_homingPrefab, transform.position, transform.rotation);
        //    bullet.transform.Rotate(Vector3.up * rotation);
        //    bullet.GetComponent<HomingProjectiles>().SetTarget(FindClosestByTag("Enemy"));
        //    rotation *= -1;
        //}
        _homingBullets = 6;

        _usedAbility = true;



        //Pawn();
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
        base.Horse();
    }
}
