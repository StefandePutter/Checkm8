using System.Collections;
using UnityEngine;

public class EnemyHorse : EnemyBase
{
    [SerializeField] private float _rotateSpeed;
    private Transform _playerTransform;

    protected override void Shoot()
    {
        //return;
    }
    void Update()
    {
        if (!_canFire) { return; }

        Transform target = GameManager.s_Player.transform;

        bool hitPlayer = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, ((target.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f)), out hit, 20, LayerMask.GetMask("Player", "Enemy", "Environment"), QueryTriggerInteraction.Ignore))
        {
            if (hit.transform == target)
            {
                // In Range and i can see player
                StartCoroutine(Charge());
                
                hitPlayer = true;
            } else
            {
                hitPlayer = false;
            }
        }

        if (hitPlayer)
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotateSpeed);
        }
        
    }

    private IEnumerator Charge()
    {
        yield return null;
    }
}
