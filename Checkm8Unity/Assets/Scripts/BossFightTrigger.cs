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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameManager.s_Instance;
        _camera = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.CompareTag("Player"));
        if (!_isTriggered)
        {
            if (other.CompareTag("Player"))
            {

                _gameManager.ChangeSpawnEnemies(false);

                _boss = Instantiate(_boss, _spawnBossPos, _gameManager.transform.rotation);

                _camera.SetTarget(_camPosBossArea);

                _isTriggered = true;
            }
        }
    }

    private void Update()
    {
        if (_isTriggered && _camera.transform.position == _camPosBossArea && !_isDone)
        {
            _isDone = true;
            StartCoroutine(_boss.EnableEnemy());
        }
    }
}
