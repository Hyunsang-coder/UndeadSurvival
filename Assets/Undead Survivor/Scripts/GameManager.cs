using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Player player;
    [SerializeField] PoolManager poolManager;

    public float GameTime { get; set; }
    [SerializeField] float maxGameTime = 2 * 10f;


    void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>().GetComponent<Player>();
        poolManager = FindObjectOfType<PoolManager>().GetComponent<PoolManager>();
    }

    private void Update()
    {
        GameTime += Time.deltaTime;

        if (GameTime > maxGameTime) {
            GameTime = maxGameTime;
        }
    }

    public Player Player
    {
        get { return player; }
        private set { player = value; }
    }

    public PoolManager Pool
    {
        get { return poolManager; }
        private set { poolManager = value; }
    }


}
