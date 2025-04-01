using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;
    static public GameObject s_Player;

    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _horsePrefab;
    [SerializeField] private GameObject _bischopPrefab;
    [SerializeField] private GameObject _rookPrefab;
    [SerializeField] private GameObject _queenPrefab;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private float _spawnTimer;
    private float _spawnTime;
    [SerializeField] private TextMeshProUGUI _timePlayerUi;
    [SerializeField] private TextMeshProUGUI _timeEnemyUi;
    private float _timePlayer;
    private float _timeEnemy;
    [SerializeField] private List<GameObject> _uiHealth;

    
    private int[] _spawnPosX = new int[11] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 };
    private bool _bossBattle;
    private Camera _camera;

    public Dictionary<int,Vector3> MovePlaces = new Dictionary<int, Vector3>(); // enemy id, target Pos
    public Image UiHorse;
    public Image UiBischop;
    public Image UiTower;
    public Image UiQueen;
    public ObjectPool PlayerBulletsPool;
    public ObjectPool EnemyBulletsPool;
    [HideInInspector] public PlayerInputManager InputManager;

    private void Awake()
    {
        s_Instance = this;
    }

    void Start()
    {
        _camera = Camera.main;
        s_Player = Instantiate(_pawnPrefab);
        s_Player.transform.SetParent(_camera.transform);
        InputManager = GetComponent<PlayerInputManager>();

        _timePlayer = 300f;
        _timeEnemy = 300f;
    }

    void Update()
    {
        if (_spawnTime <= 0)
        {
            // spawnpoint dependent on cameraPos we floor it to make them spawn exactly on tiles
            Vector3 spawnPos = new Vector3();
            spawnPos.z = Mathf.Floor(_camera.transform.position.z) + 23;
            if (spawnPos.z % 2 != 0)
            {
                spawnPos.z++;
            }

            // get random spawnpoint x position of possible spawnpoints
            spawnPos.x = _spawnPosX[Random.Range(0, _spawnPosX.Length)];

            // spawn random enemy
            int enemyIndex = Random.Range(0, _enemyPrefabs.Count);
            GameObject enemy = Instantiate(_enemyPrefabs[3], spawnPos, transform.rotation);

            _spawnTime = _spawnTimer;
        }

        // display time
        _timePlayerUi.text = Mathf.Floor(_timePlayer / 60).ToString("00") + ":" + (_timePlayer%60).ToString("00");
        _timeEnemyUi.text = Mathf.Floor(_timeEnemy / 60).ToString("00") + ":" + (_timeEnemy%60).ToString("00");

        _timePlayer -= Time.deltaTime;
        if (_bossBattle)
        {
            _timeEnemy -= Time.deltaTime;
        }

        if (_timePlayer <= 0)
        {
            // game ends when your time runs out
            GameOver();
        }

        _spawnTime -= Time.deltaTime;
    }

    public void Damage()
    {
        int i = _uiHealth.Count - 1;

        Destroy(_uiHealth[i]);
        _uiHealth.RemoveAt(i);

        if (i == 0)
        {
            // game over
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BecomePawn()
    {
        ChangePlayer(_pawnPrefab);
        s_Player.transform.SetParent(_camera.transform);
    }

    public void BecomeHorse()
    {
        ChangePlayer(_horsePrefab);
    }
    public void BecomeBischop()
    {
        ChangePlayer(_bischopPrefab);
        s_Player.transform.SetParent(_camera.transform);
    }

    public void BecomeRook()
    {
        ChangePlayer(_rookPrefab);
        s_Player.transform.SetParent(_camera.transform);
    }

    public void BecomeQueen()
    {
        ChangePlayer(_queenPrefab);
        s_Player.transform.SetParent(_camera.transform);
    }

    public void AddPlayerTime(float time)
    {
        _timePlayer += time;
    }

    private void ChangePlayer(GameObject prefab)
    {
        // get transform values can't just copy transform it will be a pointer
        Vector3 pos = s_Player.transform.position;
        Quaternion rotation = s_Player.transform.rotation;
        
        // destroy current player
        Destroy(s_Player);
        
        // spawn new chess piece player
        s_Player = Instantiate(prefab, pos, rotation);
    }
}
