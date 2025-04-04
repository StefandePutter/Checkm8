using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    private int _id;
    private static int s_id=0;
    private bool _enabled;
    [SerializeField] private float _health = 1;
    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private GameObject _deathSoundPrefab;

    protected GameManager _gameManager;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;
    [SerializeField] protected float _score;
    [SerializeField] private float _waitTime = 0.5f;
    protected int[] _fieldSpacesX;
    protected LayerMask _layerMask;


    protected bool _allowedToMove;
    protected bool _usedAbility;
    protected bool _canFire;

    protected float _timeToFire;
    protected Rigidbody _rb;

    protected virtual void Start()
    {
        _gameManager = GameManager.s_Instance;
        _fieldSpacesX = _gameManager.SpawnPosesX;
        _rb = GetComponent<Rigidbody>();
        _layerMask = LayerMask.GetMask("Enemy", "EnemyHorse","Environment");

        // giving them id for GameManager dictionary then increase the id
        _id = s_id;
        s_id++;
        _gameManager.MovePlaces.Add(_id,Vector3.up);

        _rb.AddForce(Vector3.back * _moveSpeed);
    }

    private void OnBecameVisible()
    {
        // only starts firing when on screen
        // _canFire = true;
        // StartCoroutine(EnableAfterSeconds(2));
    }

    private IEnumerator EnableEnemy()
    {
        // yield return new WaitForSeconds(waitTime);
        Instantiate(_highlightPrefab, transform.position,transform.rotation);
        yield return new WaitForSeconds(0.3f);
        _canFire = true;
        _allowedToMove = true;
    }

    private void OnBecameInvisible()
    {
        // remove enemy when out of screen
        _gameManager.MovePlaces.Remove(_id);
        
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _gameManager.CameraTransform.position) < 16 && !_enabled)
        {       
            StartCoroutine(EnableEnemy());
            _enabled = true;
        }


        // calls shoot function that will be made in child files
        if (_timeToFire <= Time.time && _canFire)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    public void TakeDamage(float amount = 1)
    {
        _health -= amount;
        // Debug.Log(name + " took " + amount + " damage" + _health + " health");
        _hitSound.Play();

        if (_health <= 0)
        {
            Die();
            
        }
    }

    protected virtual void Die()
    {
        Instantiate(_deathSoundPrefab,transform.position,transform.rotation);

        _gameManager.AddPlayerTime(_score);
        _gameManager.MovePlaces.Remove(_id);

        Destroy(gameObject);
    }

    protected IEnumerator MoveAmountDown(int amount)
    {
        _allowedToMove = false;
        amount *= 2;

        float zPos = transform.position.z - amount;
        while (transform.position.z != zPos)
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

            

            RaycastHit hit;
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 3, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(target - Vector3.up, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);
                
                _allowedToMove = true;
                yield break;
            }
            else
            {
                Debug.DrawRay(target - Vector3.up, transform.TransformDirection(Vector3.up) * 2, Color.green);

                if (!CheckTargetPosition(target))
                {
                    yield break;
                }

                while (transform.position.z != target.z)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }

                _gameManager.MovePlaces[_id] = Vector3.up;
            }

            // transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
            
            yield return null;
        }

        yield return new WaitForSeconds(_waitTime);

        _allowedToMove = true;
    }

    protected IEnumerator MoveAmountDiagonal(int xPos)
    {
        _allowedToMove = false;

        float zPos = xPos - transform.position.x;
        int multiplier = 1;
        if (zPos < 0)
        {
            multiplier = -1;
            zPos *= -1;
        }

        //Vector3 target = new Vector3(xPos, transform.position.y, transform.position.z - zPos);
        Vector3 target;
        while (transform.position.x != xPos)
        {
            target = new Vector3(transform.position.x+(2*multiplier), transform.position.y, transform.position.z - 2);

            RaycastHit hit; 
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 3, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(target - Vector3.up, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);

                _allowedToMove = true;
                yield break;
            } else
            {
                Debug.DrawRay(target - Vector3.up, transform.TransformDirection(Vector3.up) * 2, Color.green);

                if (!CheckTargetPosition(target))
                {
                    yield break;
                }

                

                while (transform.position.x != target.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }
            


            yield return null;
        }

        yield return new WaitForSeconds(_waitTime);

        _allowedToMove = true;
    }

    protected IEnumerator MoveHorizontal(int xPos)
    {
        _allowedToMove = false;

        float zPos = xPos - transform.position.x;
        int multiplier = 1;
        if (zPos < 0)
        {
            multiplier = -1;
        }

        Vector3 target;
        while (transform.position.x != xPos)
        {
            target = new Vector3(transform.position.x + (2 * multiplier), transform.position.y, transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(target - Vector3.up, transform.TransformDirection(Vector3.up), out hit, 3, _layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);

                _allowedToMove = true;
                yield break;
            }
            else
            {
                Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

                if (!CheckTargetPosition(target))
                {
                    yield break;
                }



                while (transform.position.x != target.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
                    yield return null;
                }
            }



            yield return null;
        }

        yield return new WaitForSeconds(_waitTime);

        _allowedToMove = true;
    }

    private bool CheckTargetPosition(Vector3 target)
    {

        if (!_gameManager.MovePlaces.ContainsValue(target))
        {
            _gameManager.MovePlaces[_id] = target;
            return true;
        }
        else
        {
            // piece is already moving to this square
            // Debug.Log("tried going to the same square");
            _allowedToMove = true;
            return false;
        }
    }

    protected abstract void Shoot();
}
