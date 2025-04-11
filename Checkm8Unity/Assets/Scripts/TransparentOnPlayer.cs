using UnityEngine;

public class TransparentOnPlayer : MonoBehaviour
{
    [SerializeField] private Material _transparantMaterial;
    private Material _material;

    void Start()
    {
        // set current material
        _material = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        // change material to transparent
        if (other.CompareTag("Player") || other.CompareTag("Boss"))
        {
            GetComponent<MeshRenderer>().material = _transparantMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // change back material
        if (other.CompareTag("Player") || other.CompareTag("Boss"))
        {
            GetComponent<MeshRenderer>().material = _material;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        GetComponent<MeshRenderer>().material = _transparantMaterial;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        GetComponent<MeshRenderer>().material = _material; 
    //    }
    //}
}
