using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance; // Singleton instance
    public static GameObject s_Player;

    // serialized fields
    [SerializeField] public AudioSource _gameMusic;
    [SerializeField] public AudioSource _Bossmusic;
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
    [SerializeField] private GameObject _gameOverUi;
    [SerializeField] private GameObject _gameWonUi;

    // Game state properties
    private int _spawnableEnemies = 1;
    private float _spawnTime;
    private float _timePlayer;

    [HideInInspector] public bool GameDone { get; set; }
    [HideInInspector] public float TimeEnemy { get; private set; }

    private bool _bossBattle;
    private Camera _camera;
    private bool _spawningEnemies;
    private bool _spawningPlayer;

    // Predefined X positions of board
    [HideInInspector] public int[] SpawnPosesX = new int[7];

    public Transform CameraTransform;

    // Dictionary of enemy IDs and their move targets
    public Dictionary<int, Vector3> MovePlaces = new Dictionary<int, Vector3>();

    [SerializeField] private RectTransform _uiClockSwitch;

    // UI icons for characters and abilities
    public Image UiCharIconSprite;
    public Image UiCharIcon;
    public Image UiHorse;
    public Image UiHorseAbility;
    public Image UiBischop;
    public Image UiBischopAbility;
    public Image UiTower;
    public Image UiTowerAbility;
    public Image UiQueen;
    public Image UiQueenAbility;

    // Bullet object pools
    public ObjectPool PlayerBulletsPool;
    public ObjectPool EnemyBulletsPool;

    [HideInInspector] public PlayerInputManager InputManager;

    private void Awake()
    {
        s_Instance = this; // Assign the singleton

        // set board X positions
        int[] value = new int[7] { -6, -4, -2, 0, 2, 4, 6 };
        SpawnPosesX = value;

        _spawningEnemies = true;
    }

    void Start()
    {
        _camera = Camera.main;

        // Start player spawn
        StartCoroutine(SpawnPlayer());

        InputManager = GetComponent<PlayerInputManager>();

        // set game time
        _timePlayer = 300f;
        TimeEnemy = 300f;
    }

    void Update()
    {
        // Respawn player if they fall behind the camera
        if (s_Player.transform.position.z < CameraTransform.position.z - 3 && !_spawningPlayer)
        {
            StartCoroutine(SpawnPlayer());
            Damage();
        }

        // enemy spawning
        if (_spawnTime <= 0 && _spawningEnemies)
        {
            bool spawned = false;
            while (!spawned)
            {
                Vector3 spawnPos = new Vector3();
                spawnPos.z = Mathf.Floor(_camera.transform.position.z) + 23;

                // Make sure you spawn on the grid in a tile
                if (spawnPos.z % 2 != 0)
                    spawnPos.z++;

                // Get random X position
                spawnPos.x = SpawnPosesX[Random.Range(0, SpawnPosesX.Length)];

                // Check if spawn point is clear
                if (!Physics.Raycast(spawnPos - Vector3.up, Vector3.up, out RaycastHit hit, 3f, LayerMask.GetMask("Enemy", "Environment"), QueryTriggerInteraction.Ignore))
                {
                    // Spawn random enemy from allowed list
                    int enemyIndex = Random.Range(0, _spawnableEnemies);
                    Instantiate(_enemyPrefabs[enemyIndex], spawnPos, transform.rotation);
                    spawned = true;
                }
            }

            // Reset spawn timer
            _spawnTime = _spawnTimer;
        }

        // Update UI clocks
        _timePlayerUi.text = Mathf.Floor(_timePlayer / 60).ToString("00") + ":" + (_timePlayer % 60).ToString("00");

        TimeEnemy = Mathf.Max(0, TimeEnemy);
        _timeEnemyUi.text = Mathf.Floor(TimeEnemy / 60).ToString("00") + ":" + (TimeEnemy % 60).ToString("00");

        // Update timers based on battle state
        if (_bossBattle)
        {
            TimeEnemy -= Time.deltaTime;
        }
        else
        {
            _timePlayer -= Time.deltaTime;
        }

        // Check if player time is up
        if (_timePlayer <= 0)
        {
            GameOver();
        }

        _spawnTime -= Time.deltaTime;
    }

    // Increase number of enemies that can be spawned
    public void AddEnemy()
    {
        if (_spawnableEnemies < _enemyPrefabs.Count)
        {
            _spawnableEnemies++;
            Debug.Log("added enemy " + _spawnableEnemies);
        }
    }

    // Handles player damage and UI
    public void Damage()
    {
        int i = _uiHealth.Count - 1;

        Destroy(_uiHealth[i]);
        _uiHealth.RemoveAt(i);

        if (i == 0)
        {
            GameOver();
        }
    }

    // Toggle boss battle state and adjust UI
    public void ToggleBossBattle(float time = 0f)
    {
        _bossBattle = !_bossBattle;
        float zAxis = -6.9f;
        if (_bossBattle)
        {
            zAxis *= -1;
            TimeEnemy = time;
        }

        // switch the chess clock
        _uiClockSwitch.rotation = Quaternion.Euler(0, 0, zAxis);
    }

    // Restart the current scene
    public void ResetScene()
    {
        Time.timeScale = 1;
        GameDone = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called when the game is lost
    public void GameOver()
    {
        // reset static paramaters of player
        PlayerBase.Reset();

        GameDone = true;
        _gameOverUi.SetActive(true);
        Time.timeScale = 0;
    }

    // Called when the game is won
    public void GameWon()
    {
        // reset static paramaters of player
        PlayerBase.Reset();

        GameDone = true;
        _gameWonUi.SetActive(true);
        Time.timeScale = 0;
    }

    // Change player to pawn
    public void BecomePawn()
    {
        ChangePlayer(_pawnPrefab);
    }

    // Change player to horse
    public void BecomeHorse()
    {
        ChangePlayer(_horsePrefab);
    }

    // Change player to bishop
    public void BecomeBischop()
    {
        ChangePlayer(_bischopPrefab);
    }

    // Change player to rook
    public void BecomeRook()
    {
        ChangePlayer(_rookPrefab);
    }

    // Change player to queen
    public void BecomeQueen()
    {
        ChangePlayer(_queenPrefab);
    }

    // Add time to the player's clock
    public void AddPlayerTime(float time)
    {
        _timePlayer += time;
    }

    // Remove time from the enemy's clock
    public void RemoveEnemyTime(float time)
    {
        TimeEnemy -= time;
    }

    // enable or disable enemy spawning
    public void ChangeSpawnEnemies(bool value)
    {
        _spawningEnemies = value;
    }

    // spawn the player at a valid location
    private IEnumerator SpawnPlayer()
    {
        _spawningPlayer = true;
        Vector3 spawnPos = new Vector3();
        bool spawned = false;

        while (!spawned)
        {
            spawnPos.z = Mathf.Floor(CameraTransform.position.z);

            if (spawnPos.z % 2 != 0)
                spawnPos.z++;

            if (!Physics.Raycast(spawnPos - Vector3.up, Vector3.up, out RaycastHit hit, 3f, LayerMask.GetMask("Enemy", "Environment"), QueryTriggerInteraction.Ignore))
            {
                if (s_Player != null)
                {
                    Destroy(s_Player);
                }

                s_Player = Instantiate(_pawnPrefab, spawnPos, Quaternion.identity);
                s_Player.GetComponent<PlayerBase>().SetTarget(spawnPos);

                // Hide ability icons
                UiHorseAbility.gameObject.SetActive(false);
                UiBischopAbility.gameObject.SetActive(false);
                UiTowerAbility.gameObject.SetActive(false);
                UiQueenAbility.gameObject.SetActive(false);

                spawned = true;
            }
            yield return null;
        }

        _spawningPlayer = false;
    }

    // Replaces current player with selected prefab
    private void ChangePlayer(GameObject prefab)
    {
        Vector3 pos = s_Player.transform.position;
        Quaternion rotation = s_Player.transform.rotation;

        Destroy(s_Player);
        s_Player = Instantiate(prefab, pos, rotation);
    }
}
