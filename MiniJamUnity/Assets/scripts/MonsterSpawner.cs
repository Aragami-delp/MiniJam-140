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

    private int monsterToSpawnNumber;
    public int MonsterOnScreen { get { return monsterOnScreen; } set { OnMonsterChange(value); } }

    public int BonusXpAmount;

    private void OnMonsterChange(int newOnScreen)
    {
        monsterOnScreen = newOnScreen;
        int freeSpace = monsterLimit - monsterOnScreen;


        monsterToSpawnNumber = freeSpace;
    }

    // Start is called before the first frame update
    void Start()
    {
        MonsterCleaning.OnMonsterDestroyed += OnMonsterCleaned;

        spawnArea = GetComponent<BoxCollider2D>();
        MonsterOnScreen = 0;

    }

    private void OnMonsterCleaned(object sender, bool wasHit)
    {
        Debug.Log("Clean event fired");
        MonsterOnScreen--;
    }

    private IEnumerator SpawnMonster(int timesToRun) 
    {
        for (int i = 0; i < timesToRun; i++)
        {


            float spawnAreaX = spawnArea.size.x / 2;
            float spawnAreaY = spawnArea.size.y / 2;

            Vector3 spawnPoint = Vector3.zero;
            bool freeSpawn;

            GameObject enemyToSpawn = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];

            Vector2 colliderSize = enemyToSpawn.GetComponent<BoxCollider2D>().size * enemyToSpawn.transform.localScale;

            Debug.Log(colliderSize);

            do
            {
                freeSpawn = true;
                spawnPoint.x = UnityEngine.Random.Range(-spawnAreaX, spawnAreaX);
                spawnPoint.y = UnityEngine.Random.Range(-spawnAreaY, spawnAreaY);

                Collider2D[] hits = Physics2D.OverlapBoxAll(spawnPoint, colliderSize, transform.eulerAngles.z);


                foreach (Collider2D hit in hits)
                {
                    if (hit.tag == "Enemy")
                    {
                        freeSpawn = false;
                        Debug.Log("Fuckter spawn");
                    }
                }

                yield return new WaitForSeconds(0.5f);

            }
            while (!freeSpawn);


            GameObject.Instantiate(enemyToSpawn, spawnPoint + transform.position, transform.rotation, backgroundParent);

            monsterOnScreen++;
        }
    }

    private void FixedUpdate()
    {
        if (monsterToSpawnNumber <= 0) 
        {
            return;
        }

        float spawnAreaX = spawnArea.size.x / 2;
        float spawnAreaY = spawnArea.size.y / 2;

        Vector3 spawnPoint = Vector3.zero;
        bool freeSpawn;

        GameObject enemyToSpawn = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];

        //Skeleton Should be be less likeley 
        if (enemyToSpawn.name.StartsWith("Skeleton"))
        {
           enemyToSpawn = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];
           
           if (enemyToSpawn.name.StartsWith("Skeleton"))
           {
                enemyToSpawn = monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)];
           }
        }

        Vector2 colliderSize = enemyToSpawn.transform.GetComponent<BoxCollider2D>().size;

        int timesIterated = 0;

        do
        {
            freeSpawn = true;
            spawnPoint.x = UnityEngine.Random.Range(-spawnAreaX, spawnAreaX);
            spawnPoint.y = UnityEngine.Random.Range(-spawnAreaY, spawnAreaY);

            Collider2D[] hits = Physics2D.OverlapBoxAll(spawnPoint + transform.position, colliderSize , transform.eulerAngles.z);

            timesIterated++;

            if (timesIterated >= 20) 
            {
                Debug.Log("Too many tries spawning Monster. Skip!");
                return;
            }

            foreach (Collider2D hit in hits)
            {
                if (hit.tag == "Enemy")
                {
                    freeSpawn = false;
                }
            }

            //check if Box is under y -1

            float yBound = spawnPoint.y - colliderSize.y / 2; 

            if (yBound <= -1) 
            {
                Debug.Log("Spawn to low");
                freeSpawn = false;
            }

        }
        while (!freeSpawn);


       GameObject newMonster = GameObject.Instantiate(enemyToSpawn, spawnPoint + transform.position, transform.rotation, backgroundParent);

        newMonster.GetComponent<Monster>().ScoreIncrease += BonusXpAmount;

        monsterOnScreen++;
        monsterToSpawnNumber--;
    }

    public void IncreaseBonusXp(int amount) 
    {
        BonusXpAmount += amount;
    }
}
