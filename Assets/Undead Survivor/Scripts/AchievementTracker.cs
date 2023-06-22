using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Character {UnlockedMan, UnlockedLady}
public enum PlayerSkill {Dash, Shield, WhirlWind, Vampire}

public class AchievementTracker : MonoBehaviour
{
    public static AchievementTracker Instance;
    public event Action<Character> onUnlockingCharacter;
    public event Action<PlayerSkill> onLearningSkill;



    [Header("Game objects")]
    public GameObject [] lockedCharacters;
    public GameObject [] unlockedCharacters;
    public SkillUI skillUI;


    
    Character[] characters;

    [Header("Character conditions")]
    [SerializeField] int conditionForCharOne = 100;
    [SerializeField] bool metConditionForCharOne;
    [SerializeField] bool hasUnlockCharTwo;
    [SerializeField] bool hasUnlockCharOne;



    [Header("Skill conditions")]

    [SerializeField] int conditionForDash = 10;
    [SerializeField] bool learnedDash;
    [SerializeField] int conditionForShield = 15;
    [SerializeField] bool learnedShield;
    [SerializeField] int conditionForWhirWind = 30;
    [SerializeField] bool learnedWhirWind;
    


    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        characters = (Character[])Enum.GetValues(typeof(Character));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }


    void Start()
    {   
        UnlockCharacter();   
        
        Debug.Log(characters[0].ToString() +" : " + PlayerPrefs.GetInt(characters[0].ToString()));
        Debug.Log(characters[1].ToString() +" : " + PlayerPrefs.GetInt(characters[1].ToString()));
        
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);


        foreach(Character character in characters)
        {
            PlayerPrefs.SetInt(character.ToString(), 0);
        }
    }
    
    void AchieveCharacter(int index)
    {
        if (PlayerPrefs.GetInt(characters[index].ToString()) ==1) return;

        PlayerPrefs.SetInt(characters[index].ToString(), 1);
        //Debug.Log(achieves[index].ToString() +" : " + PlayerPrefs.GetInt(achieves[index].ToString()));

        NoticeSystem.Instance.Notify(index);
    }


    void UnlockCharacter()
    {
        for(int i =0; i<lockedCharacters.Length; i++)
        {
            string character = characters[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(character) == 1;

            
            lockedCharacters[i].SetActive(!isUnlock);
            unlockedCharacters[i].SetActive(isUnlock);
        }
        
    }
    
    void Update(){
        CheckAchievements();
    }

    public void UnlockFirstCharacter()
    {
        if (hasUnlockCharOne) return;
        AchieveCharacter(0);
        hasUnlockCharOne = true;
    }
    public void UnlockSecondCharacter()
    {
        if (hasUnlockCharTwo) return;
        AchieveCharacter(1);
        hasUnlockCharTwo = true;
    }
  

    private void CheckAchievements()
    {   
        float kills = GameManager.Instance.kill;

        // Character unlocked
        if (kills == conditionForCharOne && !metConditionForCharOne)
        {
            metConditionForCharOne = true;
            //OnMeetingUnlockCondition.Invoke(0);
            onUnlockingCharacter?.Invoke(0);
            AchieveCharacter(0);
        }

        // skill unlocked 
        if (kills == conditionForDash && !learnedDash)
        {
            learnedDash = true;
            skillUI.DisplaySkill(PlayerSkill.Dash);
            onLearningSkill?.Invoke(PlayerSkill.Dash);
        }

        if (kills == conditionForShield && !learnedShield)
        {
            learnedShield = true;
            skillUI.DisplaySkill(PlayerSkill.Shield);
            onLearningSkill?.Invoke(PlayerSkill.Shield);
        }

        
        if (kills == conditionForWhirWind && !learnedWhirWind)
        {
            learnedWhirWind = true;
            skillUI.DisplaySkill(PlayerSkill.WhirlWind);
            onLearningSkill?.Invoke(PlayerSkill.WhirlWind);
        }

    }

}
