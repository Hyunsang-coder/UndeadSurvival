using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("GameObjects")]
    [SerializeField] Player player;
    [SerializeField] PoolManager poolManager;
    public ResultMenu resultMenu;
    public GameObject enemyCleaner;


    [Header("Game Control")]
    public float GameTime;
    public float MaxGameTime = 30f;
    public LevelUpMenu LevelUpModal;
    public bool isGameLive = false;

    
    [Header("PlayerInfo")]
    public int kill;
    public int XP;
    public float health;
    public float maxHealth = 100f;
    public int playerID;
    public int playerLevel;
    
    public int[] NextLvXP;

    

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        LevelUpModal = FindObjectOfType<LevelUpMenu>(true).GetComponent<LevelUpMenu>();
        resultMenu = FindObjectOfType<ResultMenu>(true).GetComponent<ResultMenu>();

        isGameLive = false;

    }


    
    private void Update()
    {
        if (!isGameLive) return;

        GameTime += Time.deltaTime;

        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
            GameTime = 0;
            ResumeGame();
            Victory();
        }

    }

    public void GameStart(int playerID) {
        health = maxHealth;
        this.playerID = playerID;

        player.gameObject.SetActive(true);
        LevelUpModal.Select(playerID%2);

        ResumeGame();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Instance.PlayBGM(true);
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
    }

    IEnumerator GameOverCoroutine(){
        
        isGameLive =false;
        //enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        StopGame();

        
        resultMenu.gameObject.SetActive(true);
        resultMenu.Lose();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
        AudioManager.Instance.PlayBGM(false);

    }


    public void Victory(){
        
        StartCoroutine(VictoryCoroutine());
    }

    IEnumerator VictoryCoroutine(){
        isGameLive =false;
        enemyCleaner.SetActive(true);

        AchievementTracker.Instance.UnlockSecondCharacter();
        
        yield return new WaitForSeconds(1f);
        StopGame();
        
        resultMenu.gameObject.SetActive(true);
        resultMenu.Win();
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
        AudioManager.Instance.PlayBGM(false);
    }

    public void RetryGame(){
        SceneManager.LoadScene(0);
    }
}
