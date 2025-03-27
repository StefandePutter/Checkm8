using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _horsePrefab;
    [SerializeField] private GameObject _bischopPrefab;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private float _spawnTime;
    [SerializeField] private TextMeshProUGUI _timePlayerUi;
    private float _timePlayer;
    [SerializeField] private TextMeshProUGUI _timeEnemyUi;
    private float _timeEnemy;
    [SerializeField] private List<GameObject> _UiHealth;
    
    private int[] _spawnPosX = new int[11] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 };
    private bool _bossBattle;
    private Camera _camera;

    private int _Health;

    static public GameObject s_player;
    public Image UiHorse;
    public Image UiBischop;
    public Image UiTower;
    public Image UiQueen;
    public ObjectPool playerBulletsPool;
    public ObjectPool enemyBulletsPool;
    [HideInInspector] public PlayerInputManager inputManager;

    void Start()
    {
        _camera = Camera.main;
        s_player = Instantiate(_pawnPrefab);
        s_player.transform.SetParent(_camera.transform);
        inputManager = GetComponent<PlayerInputManager>();

        _timePlayer = 300f;
        _timeEnemy = 300f;
    }

    void Update()
    {
        if (_spawnTime <= 0)
        {
            Vector3 spawnPos = new Vector3();
            spawnPos.z = Mathf.Floor(_camera.transform.position.z) + 25;
            if (spawnPos.z % 2 != 0)
            {
                spawnPos.z++;
            }
            spawnPos.x = _spawnPosX[Random.Range(0, _spawnPosX.Length)];

            GameObject enemy = Instantiate(_enemyPrefabs[Random.Range(0,2)], spawnPos, transform.rotation);

            _spawnTime = 1f;
        }

        _timePlayerUi.text = Mathf.Floor(_timePlayer / 60).ToString("00") + ":" + (_timePlayer%60).ToString("00");
        _timeEnemyUi.text = Mathf.Floor(_timeEnemy / 60).ToString("00") + ":" + (_timeEnemy%60).ToString("00");

        _timePlayer -= Time.deltaTime;
        if (_bossBattle)
        {
            _timeEnemy -= Time.deltaTime;
        }

        if (_timePlayer <= 0)
        {
            GameOver();
        }

        _spawnTime -= Time.deltaTime;
    }

    public void Damage()
    {
        int i = _UiHealth.Count - 1;

        Destroy(_UiHealth[i]);
        _UiHealth.RemoveAt(i);
        Debug.Log(i);
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
    }

    public void BecomeHorse()
    {
        ChangePlayer(_horsePrefab);
    }
    public void BecomeBischop()
    {
        ChangePlayer(_bischopPrefab);
    }

    public void AddPlayerTime(float time)
    {
        _timePlayer += time;
    }

    private void ChangePlayer(GameObject prefab)
    {
        // get transform values can't just copy transform it will be a pointer
        Vector3 pos = s_player.transform.position;
        Quaternion rotation = s_player.transform.rotation;
        
        // destroy current player
        Destroy(s_player);
        
        // spawn new chess piece player
        s_player = Instantiate(prefab, pos, rotation);
    }
}
