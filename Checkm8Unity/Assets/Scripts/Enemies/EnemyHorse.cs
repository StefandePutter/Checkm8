using System.Collections;
using UnityEngine;

public class EnemyHorse : EnemyBase
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _chargeSpeed;
    [SerializeField] private GameObject _indicatorPrefab;
    private bool _rotateToPlayer;
    private Transform _target;

    protected override void Shoot()
    {
        // horse doesnt shoot
        return;
    }

    void Update()
    {
        if (!_canFire) { return; }

        _target = GameManager.s_Player.transform;


        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, ((_target.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f)), out hit, 20, LayerMask.GetMask("Player", "Enemy", "Environment"), QueryTriggerInteraction.Ignore))
        {
            if (hit.transform == _target)
            {
                // in Range and clear path to player
                if (!_usedAbility)
                {
                    _rotateToPlayer = true;
                    StartCoroutine(Charge());
                    _usedAbility = true;
                }
                
            }
        }

        if (_rotateToPlayer)
        {
            // rotate to player 
            Vector3 relativePos = _target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotateSpeed);
        }
    }

    // charge towards player
    private IEnumerator Charge()
    {
        // wait till looking at player
        while (Vector3.Dot(transform.forward, (_target.position - transform.position).normalized) < 0.99f)
        {
            yield return null;
        }
        
        // show indicator
        GameObject arrows = Instantiate(_indicatorPrefab, transform);
        
        arrows.transform.SetPositionAndRotation(transform.position + Vector3.up*0.1f, transform.rotation);

        float newScale;
        float timer = 0.0f;
        // slowly scale indicator
        while (timer <= 1.5)
        {
            timer += Time.deltaTime;
            newScale = Mathf.Lerp(0, 1, timer);
            arrows.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
        
        // disable indicator
        arrows.SetActive(false);
        _rotateToPlayer = false;

        Vector3 direction = (transform.position - _target.position).normalized;
        direction.y = 0;
        
        // charge this direction till destroyed
        while (true)
        {
            transform.position += transform.forward * Time.deltaTime * _chargeSpeed;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // try to damage player
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.TryGetComponent<IDamageable>(out IDamageable component))
            {
                component.TakeDamage();
            }
        }
    }
}
