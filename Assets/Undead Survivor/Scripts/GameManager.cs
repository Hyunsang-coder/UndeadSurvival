using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObjects")]
    [SerializeField] Player player;
    [SerializeField] PoolManager poolManager;
    public Result resultMenu;
    public GameObject enemyCleaner;


    [Header("Game Control")]
    public float GameTime;
    public float MaxGameTime = 30f;

    public LevelUp LevelUpModal;

    public bool isGameLive = false;

    
    [Header("PlayerInfo")]
    public int kill;
    public int XP;
    public float health;
    public float maxHealth = 100f;
    public int playerID;



    public int playerLevel;

    public int stageLevel;

    public int[] NextLvXP;
    void Awake()
    {
        Instance = this;
        LevelUpModal = FindObjectOfType<LevelUp>(true).GetComponent<LevelUp>();
        resultMenu = FindObjectOfType<Result>(true).GetComponent<Result>();

        isGameLive = false;

    }

    public void GameStart(int playerID) {
        health = maxHealth;
        this.playerID = playerID;

        player.gameObject.SetActive(true);
        LevelUpModal.Select(playerID%2);
        

        ResumeGame();
    }

    private void Update()
    {
        if(!isGameLive) return;
        
        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime) {
            GameTime = MaxGameTime;
            stageLevel ++;
            GameTime = 0;
            ResumeGame();
            Victory();
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
        if(!isGameLive) return;

        XP++;
        if (XP == NextLvXP[Mathf.Min(playerLevel, NextLvXP.Length-1)])
        {
            playerLevel++;
            XP = 0;

            LevelUpModal.Show();
        }
    }
    public void GainKillScore()
    {
        if(!isGameLive) return;

        kill++;
    }

    public void StopGame(){
        isGameLive = false;
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        isGameLive = true;
        Time.timeScale = 1;
    }

    public void GameOver(){
        StartCoroutine(GameOverCoroutine());

        resultMenu.gameObject.SetActive(true);
    }

    IEnumerator GameOverCoroutine(){
        isGameLive = false;
        yield return new WaitForSeconds(0.5f);

        StopGame();
    }


    public void Victory(){
        StartCoroutine(VictoryCoroutine());

        resultMenu.gameObject.SetActive(true);
        resultMenu.Win();
    }

    IEnumerator VictoryCoroutine(){
        
        isGameLive = false;
        enemyCleaner.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        StopGame();
    }

    public void RetryGame(){
        SceneManager.LoadScene(0);
    }
}
