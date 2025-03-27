using UnityEngine;

public class EnemyPawn : EnemyBase, IDamageable
{
    private Coroutine moveCoroutine;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_moving)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 2))

            {
                // Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            } else
            {
                StartCoroutine(MoveAmountDown(1));
            }

        }
    }

    protected override void Shoot()
    {
        GameObject bullet = _gameManager.enemyBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
}
