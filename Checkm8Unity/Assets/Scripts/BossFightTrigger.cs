using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _camPosBossArea;
    [SerializeField] private Vector3 _spawnBossPos;
    [SerializeField] private EnemyBase _boss;
    private bool _isTriggered;
    private bool _isDone;
    private GameManager _gameManager;
    private CameraMovement _camera;

    void Start()
    {
        _gameManager = GameManager.s_Instance;
        _camera = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isTriggered)
        {
            if (other.CompareTag("Player"))
            {
                // stop spawning enemies
                _gameManager.ChangeSpawnEnemies(false);

                // spawn boss
                _boss = Instantiate(_boss, _spawnBossPos, _gameManager.transform.rotation);

                // set camera to go to target
                _camera.SetTarget(_camPosBossArea);

                _isTriggered = true;
            }
        }
    }

    private void Update()
    {
        // when camera is at arena
        if (_isTriggered && _camera.transform.position == _camPosBossArea && !_isDone)
        {
            // only trigger once
            _isDone = true;
            
            // enable boss
            StartCoroutine(_boss.EnableEnemy());
        }
    }
}
