using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _horsePrefab;

    static public GameObject s_player;
    public ObjectPool s_playerBulletsPool;
    public ObjectPool s_enemyBulletsPool;
    [HideInInspector] public PlayerInputManager inputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s_player = Instantiate(_pawnPrefab);
        inputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
