using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;
    public static GameObject s_Player;

    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _horsePrefab;
    [SerializeField] private GameObject _bischopPrefab;
    [SerializeField] private GameObject _rookPrefab;
    [SerializeField] private GameObject _queenPrefab;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private float _spawnTimer;
    [SerializeField] private TextMeshProUGUI _timePlayerUi;
    [SerializeField] private TextMeshProUGUI _timeEnemyUi;
    [SerializeField] private List<GameObject> _uiHealth;

    private float _spawnTime;
    private float _timePlayer;
    private float _timeEnemy;
    private bool _bossBattle;
    private Camera _camera;
    private bool _spawningEnemies;

    [HideInInspector] public int[] SpawnPosesX = new int[7];
    public Transform CameraTransform;
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
        int[] value = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
        SpawnPosesX = value;
        _spawningEnemies = true;
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
        if (_spawnTime <= 0 && _spawningEnemies)
        {
            //for (int i = 0; i < 1; i++)
            //{
                
            //}
            bool spawned = false;
            while (!spawned)
            {
                Vector3 spawnPos = new Vector3();
                spawnPos.z = Mathf.Floor(_camera.transform.position.z) + 23;
                if (spawnPos.z % 2 != 0)
                {
                    spawnPos.z++;
                }

                // get random spawnpoint x position of possible spawnpoints
                spawnPos.x = SpawnPosesX[Random.Range(0, SpawnPosesX.Length)];

                RaycastHit hit;
                if (Physics.Raycast(spawnPos - Vector3.up, Vector3.up, out hit, 3f, LayerMask.GetMask("Enemy", "Environment"), QueryTriggerInteraction.Ignore))
                {

                }
                else
                {
                    // spawn random enemy
                    int enemyIndex = Random.Range(0, _enemyPrefabs.Count - 1);
                    GameObject enemy = Instantiate(_enemyPrefabs[enemyIndex], spawnPos, transform.rotation);
                    spawned = true;
                }
            }

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
        PlayerBase.Reset();
        
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

    public void ChangeSpawnEnemies(bool value)
    {
        _spawningEnemies = value;
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
