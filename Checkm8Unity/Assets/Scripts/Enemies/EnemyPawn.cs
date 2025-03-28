using UnityEngine;

public class EnemyPawn : EnemyBase
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_allowedToMove)
        {
            // RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            // Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
            //if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))

            //{
            //    Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            //    Debug.Log(hit.collider.gameObject.name + "Did Hit");
            //} else
            //{
            //    Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.yellow);
            //
            //   StartCoroutine(MoveAmountDown(1));
            //}

            StartCoroutine(MoveAmountDown(1));
        }
    }

    protected override void Shoot()
    {
        GameObject bullet = _gameManager.EnemyBulletsPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.SetActive(true);
        }
        StartCoroutine(ObjectPool.DisableAfterSec(bullet, 0.3f));
    }
}
