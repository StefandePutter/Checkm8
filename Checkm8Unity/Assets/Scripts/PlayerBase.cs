using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float shootSpeed;

    protected float timeToFire;
    protected Vector2 moveValue;
    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(moveValue.x * moveSpeed, 0, moveValue.y * moveSpeed));



        if (timeToFire <= Time.time)
        {
            Shoot();
            timeToFire = Time.time + shootSpeed;
        }
    }

    private void OnMove(InputValue input)
    {
        moveValue = input.Get<Vector2>();
    }

    protected abstract void Shoot();
}
