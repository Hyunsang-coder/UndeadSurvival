using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObjects")]
    [SerializeField] Player player;
    [SerializeField] PoolManager poolManager;


    [Header("Game Control")]
    public float GameTime;
    public float MaxGameTime;

    
    [Header("PlayerInfo")]
    public int kill;
    public int XP;
    public int helath;
    public int maxHealth = 100;

    public int playerLevel;

    public int stageLevel;

    public int[] NextLvXP;
    void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>().GetComponent<Player>();
        poolManager = FindObjectOfType<PoolManager>().GetComponent<PoolManager>();

    }

    private void Start() {
        helath = maxHealth;
    }

    private void Update()
    {
        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime) {
            GameTime = MaxGameTime;
            stageLevel ++;
            GameTime = 0;
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

    public void GainXP()
    {
        XP++;
        if (XP == NextLvXP[playerLevel])
        {
            playerLevel++;
            XP = 0;

            //out of bounds 에러 방지용 
            playerLevel = Mathf.Clamp(playerLevel, 0, NextLvXP.Length -1);
        }
    }
}
