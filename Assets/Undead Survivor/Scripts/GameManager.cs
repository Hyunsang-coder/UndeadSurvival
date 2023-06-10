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

    public LevelUp LevelUpModal;

    public bool isGameLive = true;

    
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

        // temporary
        LevelUpModal.Select(0);
    }

    private void Update()
    {
        if(!isGameLive) return;
        
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
        if (XP == NextLvXP[Mathf.Min(playerLevel, NextLvXP.Length-1)])
        {
            playerLevel++;
            XP = 0;

            LevelUpModal.Show();
        }
    }

    public void StopGame(){
        isGameLive = false;
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        isGameLive = true;
        Time.timeScale = 1;
    }
}
