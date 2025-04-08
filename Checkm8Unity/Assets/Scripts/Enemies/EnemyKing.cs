using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyKing : EnemyBase
{
    [SerializeField] private EnemyBase[] _enemies;
    [SerializeField] private float _raiseSpeed;

    private List<EnemyBase> _wave = new List<EnemyBase>();

    private int[][ ] _waves = new int[9][ ];

    private int _spawnWave = 0;
    private bool _randomWave;

    protected override void Start()
    {
        base.Start();
        // enemies: 0 = pawn, 1 = Horse, 2 = Bischop, 3 = Rook 
        _waves[0] = new int[3] { 0,0,0 };
        _waves[1] = new int[3] { 1,1,1 };
        _waves[2] = new int[3] { 2,2,2 };
        _waves[3] = new int[3] { 3,3,3 };
        _waves[4] = new int[3] { 1,2,3 };
        _waves[5] = new int[3] { 0,2,0 };
        _waves[6] = new int[4] { 2,0,2,0 };
        _waves[7] = new int[4] { 1,3,1,3 };
        _waves[8] = new int[4] { 3,2,3,2 };

        _gameManager.ToggleBossBattle();
    }

    public override void TakeDamage(float amount = 1)
    {
        amount *= 10;

        _gameManager.RemoveEnemyTime(amount);

        if (_gameManager.TimeEnemy <= 0)
        {
            _gameManager.GameOver();
        }
    }

    protected override void Shoot()
    {
        return;
    }

    private void Update()
    {
        // if no current wave
        if (_wave.Count == 0)
        {
            if (_randomWave)
            {
                _spawnWave = Random.Range(6, 9);
            }

            SpawnWave(_waves[_spawnWave]);
            _spawnWave++;

            if (_spawnWave == 8)
            {
                _randomWave = true;
            }
        }

        // remove dead enemies from _wave list
        EnemyBase[] itemsToRemove = new EnemyBase[_wave.Count];
        for (int i = 0; i < itemsToRemove.Length; i++)
        {
            if (_wave[i] == null)
            {
                itemsToRemove[i] = _wave[i];
            }
        }
        foreach (EnemyBase item in itemsToRemove)
        {
            _wave.Remove(item);
        }

        // did it like this first but its bad practice
        // .ForEach uses a for loop to iterate so we can remove items while looping through
        //_wave.ForEach(x =>
        //{
        //    if (x == null)
        //    {
        //        _wave.Remove(x);
        //    }
        //});
    }

    private void SpawnWave(int[] wave)
    {
        Vector3[] spawnPosses = new Vector3[wave.Length];

        for (int i = 0; i < wave.Length; i++)
        {
            bool picking = true;
            while (picking)
            {
                int random = Random.Range(0, _fieldSpacesX.Length);
                int xPos = _fieldSpacesX[random];
                int zPos = Random.Range(0, 2)*2;
                Vector3 spawnPos = new Vector3(xPos, -3, 448-zPos);
                if (!spawnPosses.Contains(spawnPos))
                {
                    spawnPosses[i] = spawnPos;

                    EnemyBase minion = Instantiate(_enemies[wave[i]], spawnPos, transform.rotation);
                    minion.ManuallyActivate = true;

                    _wave.Add(minion);

                    picking = false;
                }
            }
            
        }
        
        foreach(EnemyBase minion in _wave)
        {
            Debug.Log(minion.name);

            StartCoroutine(ActivateMionion(minion));

        }
    }

    private IEnumerator ActivateMionion(EnemyBase minion)
    {
        Transform minionTransform = minion.transform;

        Collider minionCollider = minion.GetComponent<Collider>();

        minionCollider.enabled = false;
        Vector3 target = new Vector3(minionTransform.position.x, 0, minionTransform.position.z);
        while (minionTransform.position.y != 0)
        {
            minionTransform.position = Vector3.MoveTowards(minionTransform.position, target, Time.deltaTime * _raiseSpeed);
            yield return null;
        }

        minionCollider.enabled = true;
        StartCoroutine(minion.EnableEnemy());
    }
}
