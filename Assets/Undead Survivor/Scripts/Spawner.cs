using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    // SpawnData도 MonoBehavior에서 상속된 컴포넌트가 아니기에 GetComponents쓸 수 없음.
    [SerializeField] SpawnData[] spawnData;
    [SerializeField] float levelTime;
    float spawnTimer;
    float stageTimer;

    [SerializeField] float stageDuration = 10f; 
    bool isLevelingUp;

    int level;

    SpawnData enemySpawnData;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        //levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
        
        enemySpawnData = GenerateSpawnData(level);
    }

    private void Start() {
        SpawnEnemy(enemySpawnData);   
    }
    void Update()
    {
        if(!GameManager.Instance.isGameLive) return;

        spawnTimer += Time.deltaTime;
        stageTimer += Time.deltaTime;

        if (spawnTimer > enemySpawnData.spawnTime)
        {
            SpawnEnemy(enemySpawnData);
            spawnTimer = 0;
        }

        //level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime/ levelTime), spawnData.Length - 1);

        if (stageTimer > stageDuration)
        {
            level ++;   
            isLevelingUp = true;

            if (isLevelingUp)
            {
                isLevelingUp = false;
                enemySpawnData = GenerateSpawnData(level); 
                stageTimer = 0;               
            }
            //SpawnEnemy();
        }

        
    }

    void SpawnEnemy(SpawnData data){
        // 에너미 가져와 스폰
        GameObject enemy = GameManager.Instance.Pool.GetObject(0);

        // 스폰된 에너미 위치를 스폰 포인트 중 하나에 assign. (FYI, spawnPoints[0]은 spawner의 트랜스폼)
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].transform.position;
        
        
        //enemy.GetComponent<Enemy>().EnemyInint(spawnData[level]);

        enemy.GetComponent<Enemy>().EnemyInint(data);
    }


    private SpawnData GenerateSpawnData(int level)
    {   
        SpawnData newData = new SpawnData();
        newData.enemyType = level%2;
        newData.spawnTime = Mathf.Clamp(newData.spawnTime - (0.1f * level)/2f, 0.3f, newData.spawnTime);

        if (level ==0 || level % 5 != 0)
        {
            
            newData.speed = newData.speed + (0.1f * level)/7f;
            newData.maxHealth = newData.maxHealth * (1.05f *level);
        }
        else
        {
            float speedMultiplier =2.2f;
            float healthMltiplier = 0.2f;
            newData.speed = (newData.speed + (0.1f * level)/7f) * speedMultiplier;
            newData.maxHealth = Mathf.Clamp(newData.maxHealth * (1.05f *level) * healthMltiplier, newData.maxHealth, newData.maxHealth*5);
        }
        return newData;

    }
}




[System.Serializable]

public class SpawnData
{
    public float spawnTime;

    public float maxHealth;
    public float speed;
    public int enemyType;

    public SpawnData()
    {
        spawnTime = 1f;
        maxHealth = 10f;
        speed = 2f;
        enemyType =0;
    }
}