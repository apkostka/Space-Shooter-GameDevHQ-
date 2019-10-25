using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _enemySpawnRate = 1.5f;
    [SerializeField]
    private float _powerupSpawnRateBase = 7f;
    private float _powerupSpawnRateRange = 2f;

    [SerializeField]
    private int _maxEnemies = 8;

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= _maxEnemies)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-13f, 13f), 8, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 8, 0);
            int randomPowerup = Random.Range(0, powerups.Length);
            GameObject newPowerup = Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(_powerupSpawnRateBase - _powerupSpawnRateRange, _powerupSpawnRateBase + _powerupSpawnRateRange));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
