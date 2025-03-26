using UnityEngine;

public class TransparentOnPlayer : MonoBehaviour
{
    [SerializeField] private Material _transparantMaterial;
    private Material _material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = _transparantMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = _material;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = _transparantMaterial;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GetComponent<MeshRenderer>().material = _material; 
        }
    }
}
