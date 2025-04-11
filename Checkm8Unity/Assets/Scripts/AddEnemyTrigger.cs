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
                // add enemy to pool
                _gameManager.AddEnemy();
                
                // dont go again
                _isTriggered = true;
            }
        }
    }
}
