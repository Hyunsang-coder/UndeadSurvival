using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchievementTracker : MonoBehaviour
{
    public static AchievementTracker Instance {get; private set;}
    public GameObject [] lockedCharacters;
    public GameObject [] unlockedCharacters;

    public event Action<int> onUnlockingCharacter;
    public event Action<int> onLearningSkill;

    enum Achieve {UnlockedMan, UnlockedLady}
    Achieve[] achieves;


    [SerializeField] int killsToUnlockCharacter1 = 100;
    [SerializeField] bool metCondition1;
    [SerializeField] bool dashLearned;
    [SerializeField] bool shieldLearned;
    
    [SerializeField] bool hasUnlockSecondCharacter;
    [SerializeField] bool hasUnlockFirstCharacter;

    private void Awake() {
        Instance = this;
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }



    void Start()
    {   
        UnlockCharacter();   
        
        Debug.Log(achieves[0].ToString() +" : " + PlayerPrefs.GetInt(achieves[0].ToString()));
        Debug.Log(achieves[1].ToString() +" : " + PlayerPrefs.GetInt(achieves[1].ToString()));
        
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);


        foreach(Achieve achive in achieves)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }
    
    void AchiveMileStone(int index)
    {
        if (PlayerPrefs.GetInt(achieves[index].ToString()) ==1) return;

        PlayerPrefs.SetInt(achieves[index].ToString(), 1);
        //Debug.Log(achieves[index].ToString() +" : " + PlayerPrefs.GetInt(achieves[index].ToString()));

        NoticeSystem.Instance.Notify(index);
    }


    void UnlockCharacter()
    {
        for(int i =0; i<lockedCharacters.Length; i++)
        {
            string character = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(character) == 1;

            
            lockedCharacters[i].SetActive(!isUnlock);
            unlockedCharacters[i].SetActive(isUnlock);

        }
        
    }
    
    void Update(){
        CheckAchievements();
    }

    public void UnlockSecondCharacter()
    {
        hasUnlockSecondCharacter = true;
        AchiveMileStone(1);
    }
  

    private void CheckAchievements()
    {
        // Character unlocked
        if (GameManager.Instance.kill == killsToUnlockCharacter1 && !metCondition1)
        {
            metCondition1 = true;
            //OnMeetingUnlockCondition.Invoke(0);
            onUnlockingCharacter?.Invoke(0);
            AchiveMileStone(0);
        }

        // skill unlocked 
        if (GameManager.Instance.kill== 10 && !dashLearned)
        {
            dashLearned = true;
            SkillUI.Instance.LearnSkill(SkillUI.PlayerSkill.Dash);
            onLearningSkill?.Invoke(0);
        }

        if (GameManager.Instance.kill== 20 && !shieldLearned)
        {
            shieldLearned = true;
            SkillUI.Instance.LearnSkill(SkillUI.PlayerSkill.HolyShield);
            onLearningSkill?.Invoke(1);
        }

        /*
        if (kill == 30 && !whirlWindLearned)
        {
            whirlWindLearned = true;
            SkillManager.Instance.LearnSkill(SkillManager.PlayerSkill.WirlWind);
        }
        */

    }

}
