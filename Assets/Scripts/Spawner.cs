using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool bAlwaysSpawn;
    [SerializeField] private int iTriggerSize = 1;
    [SerializeField] private float fTimeBetweenWaves;
    [SerializeField] private float fSpawnInterval;
    [SerializeField] private int iNumOfEnemies;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;


    bool canSpawn;

    private BoxCollider _trigger;
    private int iEnemiesSpawned;

    private void Start() 
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.size = new Vector3(iTriggerSize, 1, iTriggerSize);

        _spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = transform.GetChild(i).transform;
        }

        if (bAlwaysSpawn)
        {
            StartCoroutine(StartSpawning());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && !bAlwaysSpawn)
        {
            StartCoroutine(StartSpawning());
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && !bAlwaysSpawn)
        {
            StopSpawning();
        }
    }

    private IEnumerator StartSpawning()
    {
        canSpawn = true;

        while(canSpawn)
        {
            yield return new WaitForSeconds(fSpawnInterval);

            GameObject enemy = _enemies[Random.Range(0, _enemies.Length)];
            Transform place = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            Instantiate(enemy, place.position, place.rotation);

            iEnemiesSpawned++;

            if (iEnemiesSpawned == iNumOfEnemies)
            {
                yield return new WaitForSeconds(fTimeBetweenWaves);
                iEnemiesSpawned = 0;
            }
        }
    }

    public void StopSpawning()
    {
        if (bAlwaysSpawn)
        {
            return;
        }

        canSpawn = false;
    }

    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(iTriggerSize, 1, iTriggerSize));    
    }
}
