using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform backgroundParent;

    [SerializeField]
    private BoxCollider2D spawnArea;

    [SerializeField]
    private GameObject[] monsterPrefabs;

    [SerializeField]
    [Range(0, 10)]
    private int monsterLimit;
    
    [SerializeField]
    private int monsterOnScreen;
    public int MonsterOnScreen { get { return monsterOnScreen; } set { OnMonsterChange(monsterLimit - value); } }

   
    private void OnMonsterChange(int freeSpace)
    {
        for (int i = 0; i <= freeSpace; i++)
        {
            SpawnMonster();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        MonsterOnScreen = 0;
    }

    private void SpawnMonster() 
    {
        float spawnAreaX =  spawnArea.size.x / 2;
        float spawnAreaY = spawnArea.size.y / 2;

        Vector3 spawnPoint = Vector3.zero;

        spawnPoint.x = UnityEngine.Random.Range(-spawnAreaX, spawnAreaX);
        spawnPoint.y = UnityEngine.Random.Range(-spawnAreaY, spawnAreaY);

        GameObject enemyToSpawn = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];

        GameObject.Instantiate(enemyToSpawn, spawnPoint + transform.position ,transform.rotation, backgroundParent);
        monsterOnScreen++;
    }

}
