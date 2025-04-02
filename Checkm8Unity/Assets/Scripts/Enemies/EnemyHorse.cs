using System.Collections;
using UnityEngine;

public class EnemyHorse : EnemyBase
{
    [SerializeField] private float _rotateSpeed;
    private Transform _playerTransform;

    protected override void Start()
    {
        base.Start();
        //_playerTransform = GameManager.s_Player.transform;
    }

    protected override void Shoot()
    {
        return;
    }
    void Update()
    {
        Transform target = GameManager.s_Player.transform;

        bool hitPlayer = false;

        RaycastHit hit;
        //if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.TransformDirection(target.position), out hit, Mathf.Infinity, LayerMask.GetMask("Player", "Enemy", "Environment"), QueryTriggerInteraction.Ignore))
        //{
        //    // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);
        //    Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.TransformDirection(target.position) * hit.distance, Color.green);
        //    Debug.Log("hit " + hit.transform.tag + " " + hit.distance);
        //    hitPlayer = true;
        //} else
        //{
        //    // Debug.DrawRay(transform.position, target.position * 100, Color.green);
        //    Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.TransformDirection(target.position) * 100, Color.green);

        //}
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, ((target.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f)), out hit, Mathf.Infinity, LayerMask.GetMask("Player", "Enemy", "Environment"), QueryTriggerInteraction.Ignore))
        {
            Debug.Log("hit " + hit.transform.name);
            if (hit.transform == target)
            {
                // In Range and i can see you!
                Debug.Log("hit player");
                // Charge();
                
                hitPlayer = true;
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
