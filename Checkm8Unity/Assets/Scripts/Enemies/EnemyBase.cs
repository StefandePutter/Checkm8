using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    private int _id;
    private float _health;
    private static int s_id=0;

    protected GameManager _gameManager;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _shootSpeed;
    [SerializeField] protected float _score;
    [SerializeField] private float _waitTime = 0.5f;
    protected LayerMask _layerMask;


    protected bool _allowedToMove;
    protected bool _usedAbility;

    protected float _timeToFire;
    protected Rigidbody _rb;

    private Vector3 _leftPos;

    private void Start()
    {
        _gameManager = GameManager.s_Instance;
        _rb = GetComponent<Rigidbody>();
        _layerMask = LayerMask.GetMask("Enemy", "Environment");

        _health = 1;

        // giving them id for dictionary
        _id = s_id;
        s_id++;
        _gameManager.MovePlaces.Add(_id,Vector3.up);

        _rb.AddForce(Vector3.back * _moveSpeed);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("bye");

        _gameManager.MovePlaces.Remove(_id);
        
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        if (_timeToFire <= Time.time)
        {
            Shoot();
            _timeToFire = Time.time + _shootSpeed;
        }
    }

    public virtual void TakeDamage(float amount = 1)
    {
        Debug.Log(name + " took " + amount + " damage");
        _health = -amount;

        if (_health <= 0)
        {
            _gameManager.AddPlayerTime(_score);
            _gameManager.MovePlaces.Remove(_id);

            Destroy(gameObject);
        }
    }

    protected IEnumerator MoveAmountDown(int amount)
    {
        _allowedToMove = true;
        amount *= 2;

        float zPos = transform.position.z - amount;
        while (transform.position.z != zPos)
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

            

            RaycastHit hit;
            if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                //Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);
                _allowedToMove = false;
                yield break;
            }
            else
            {
                // Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

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

        _allowedToMove = false;
    }

    protected IEnumerator MoveAmountDiagonal(int xPos)
    {
        _allowedToMove = true;

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
            if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                //Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);

                _allowedToMove = false;
                yield break;
            } else
            {
                //Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

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

        _allowedToMove = false;
    }

    protected IEnumerator MoveHorizontal(int xPos)
    {
        _allowedToMove = true;

        float zPos = xPos - transform.position.x;
        int multiplier = 1;
        if (zPos < 0)
        {
            multiplier = -1;
        }

        //Vector3 target = new Vector3(xPos, transform.position.y, transform.position.z - zPos);
        Vector3 target;
        while (transform.position.x != xPos)
        {
            target = new Vector3(transform.position.x + (2 * multiplier), transform.position.y, transform.position.z);

            RaycastHit hit;
            if (Physics.Raycast(target, transform.TransformDirection(Vector3.up), out hit, 2, _layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * hit.distance, Color.green);

                //Debug.Log(hit.collider.gameObject.name + " got Hit by " + gameObject.name);

                _allowedToMove = false;
                yield break;
            }
            else
            {
                //Debug.DrawRay(target, transform.TransformDirection(Vector3.up) * 2, Color.green);

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

        _allowedToMove = false;
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
            _allowedToMove = false;
            return false;
        }
    }

    protected abstract void Shoot();
}
