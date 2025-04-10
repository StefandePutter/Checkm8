using UnityEngine;

public class AddEnemyTrigger : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isTriggered;

    void Start()
    {
        _gameManager = GameManager.s_Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isTriggered)
        {
            if (other.CompareTag("Player"))
            {

                _gameManager.AddEnemy();

                Debug.Log("is triggered");

                _isTriggered = true;
            }
        }
    }
}
