using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnlockManager : MonoBehaviour
{
    public GameObject [] lockedCharacters;
    public GameObject [] unlockedCharacters;

    public GameObject notice;

    enum Achieve {UnlockedMan, UnlockedLady}
    Achieve[] achieves;

    private void Awake() {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }

    }

    void AchiveMileStone(int index)
    {
        PlayerPrefs.SetInt(achieves[index].ToString(), 1);
        Debug.Log(achieves[index].ToString() +" : " + PlayerPrefs.GetInt(achieves[index].ToString()));

        StartCoroutine(ActivateNotice(index));
    }

    void Start()
    {   
        UnlockCharacter();   
        GameManager.Instance.OnMeetingUnlockCondition += AchiveMileStone;
        
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

    IEnumerator ActivateNotice(int index){

        Transform child = notice.transform.GetChild(index);

        child.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        
        notice.SetActive(true);

        yield return new WaitForSecondsRealtime(3);

        child.gameObject.SetActive(true);

        notice.SetActive(false);
    }

    /*
    private void LateUpdate() {
        foreach (Achieve achieve in achieves)
        {
            CheckAchivement(achieve);
        }
    }

    void CheckAchivement(Achieve achieve)
    {
        bool isAchieved = false;
        switch(achieve)
        {
            case Achieve.UnlockedMan:
                isAchieved = GameManager.Instance.kill >= 10;
                break;

            case Achieve.UnlockedWoman:
                isAchieved = GameManager.Instance.GameTime == GameManager.Instance.MaxGameTime;
                break;
        }
        
        if (isAchieved && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
        }

    }
    */

}
