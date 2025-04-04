using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    private bool _isTriggered;
    private GameManager _gameManager;
    private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameManager.s_Instance;
        _camera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.CompareTag("Player"));
    }
}
