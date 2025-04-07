using System.Collections;
using UnityEngine;

public class PlayerQueen : PlayerBase
{
    [SerializeField] LayerMask _nukeLayerMask;

    protected override void Shoot()
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

    public override void Queen()
    {
        s_currentQueenCooldown = _queenCooldown;
        Nuke();
    }

    private void Nuke()
    {
        Collider[] Hits = new Collider[100];

        Vector3 startPos = transform.position + Vector3.up * 0.2f;

        int hits = Physics.OverlapSphereNonAlloc(startPos, 25, Hits, _nukeLayerMask);

        _nukeLayerMask += LayerMask.GetMask("ProjectilePlayer", "ProjectileEnemy");

        for (int i = 0; i < hits; i++)
        {
            //if (Hits[i].TryGetComponent)
            float distance = Vector3.Distance(startPos, Hits[i].transform.position);

            _nukeLayerMask += LayerMask.GetMask("Environment");

            Debug.Log($"Player Would hit {Hits[i].name} for {1}");
            if (Hits[i].CompareTag("Bullet"))
            {
                Hits[i].attachedRigidbody.gameObject.SetActive(false);
            }
            else if (Hits[i].TryGetComponent<IDamageable>(out IDamageable component))
            {
                component.TakeDamage(4);
            }
        }
    }

    public override void Horse()
    {
        s_currentQueenCooldown = _queenCooldown;
        base.Horse();
    }
}
