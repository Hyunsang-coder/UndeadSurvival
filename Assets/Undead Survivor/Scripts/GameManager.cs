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
    [SerializeField] float gameTime;
    [SerializeField] float maxGameTime = 2 * 10f;

    public float GameTime { get; private set; }

    [Header("PlayerInfo")]
    public int kill;
    public int XP;

    

    public int Level;
    public int[] NextLvXP;
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

    public void GainXP()
    {
        kill++;
        XP++;
        if (XP == NextLvXP[Level])
        {
            Level++;
            XP = 0;

            //out of bounds 에러 방지용 
            Level = Mathf.Clamp(Level, 0, NextLvXP.Length -1);
        }
    }
}
