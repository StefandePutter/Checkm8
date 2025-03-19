using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
