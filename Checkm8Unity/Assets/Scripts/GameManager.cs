using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _horsePrefab;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private float _spawnTime;
    [SerializeField] private List<GameObject> _UiHealth;
    private int[] _spawnPosX = new int[11] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 };
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
        s_player = Instantiate(_pawnPrefab);
        inputManager = GetComponent<PlayerInputManager>();
        _camera = Camera.main;
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

            GameObject enemy = Instantiate(_enemyPrefabs[0], spawnPos, transform.rotation);

            _spawnTime = 1f;
        }

        _spawnTime -= Time.deltaTime;
    }

    public void Damage()
    {
        Destroy(_UiHealth[_UiHealth.Count].gameObject);
        _UiHealth.RemoveAt(_UiHealth.Count);
    }

    public void BecomePawn()
    {
        ChangePlayer(_pawnPrefab);
    }

    public void BecomeHorse()
    {
        ChangePlayer(_horsePrefab);
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
