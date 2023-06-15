using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    // SpawnData도 MonoBehavior에서 상속된 컴포넌트가 아니기에 GetComponents쓸 수 없음.
    [SerializeField] SpawnData[] spawnData;
    [SerializeField] float levelTime;
    float timer;
    int level;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
    }
    void Update()
    {
        if(!GameManager.Instance.isGameLive) return;
        
        timer += Time.deltaTime;

        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime/ levelTime), spawnData.Length - 1);

        if (timer > (spawnData[level].spawnTime))
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy(){
        // 에너미 가져와 스폰
        GameObject enemy = GameManager.Instance.Pool.GetObject(0);

        // 스폰된 에너미 위치를 스폰 포인트 중 하나에 assign. (FYI, spawnPoints[0]은 spawner의 트랜스폼)
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].transform.position;
        enemy.GetComponent<Enemy>().EnemyInint(spawnData[level]);
    }
}


[System.Serializable]

public class SpawnData
{
    public float spawnTime;

    public float maxHealth;
    public float speed;
    public int enemyType;

}