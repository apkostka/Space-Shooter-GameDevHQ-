using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _spawnRate = 2f;

    [SerializeField]
    private int _maxEnemies = 5;

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)// & GameObject.FindGameObjectsWithTag("Enemy").Length < _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-13f, 13f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
