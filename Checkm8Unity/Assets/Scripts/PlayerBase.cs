using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(Rigidbody))]
public abstract class PlayerBase : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float shootSpeed;
    
    protected InputManager inputManager;
    protected float timeToFire;
    protected Vector2 moveValue;
    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(inputManager.MoveValue.x * moveSpeed, 0, inputManager.MoveValue.y * moveSpeed));



        if (timeToFire <= Time.time)
        {
            Shoot();
            timeToFire = Time.time + shootSpeed;
        }
    }

    protected abstract void Shoot();
}
