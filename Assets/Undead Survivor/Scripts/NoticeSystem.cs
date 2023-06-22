using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoticeSystem : MonoBehaviour
{
    public static NoticeSystem Instance;
    private void Awake() {
        Instance = this;
    }

    public void Notify(int index)
    {
        StartCoroutine(ActivateNotice(index));
    }

    IEnumerator ActivateNotice(int index){
        
        //activate background image
        Transform background = transform.GetChild(0);
        background.gameObject.SetActive(true);

        Transform text = transform.GetChild(1);
        text.gameObject.SetActive(true);

        string noticeText = "";
        switch(index)
        {
            case(0):
                noticeText = "Congrats!\nCharacter unlocked.";
                break;
            case(1):
                noticeText = "Congrats!\nCharacter unlocked.";
                break;
            case(2):
                noticeText = "Skill(Dash) unlocked.\nPress <Space>.";
                break;
            case(3):
                noticeText = "Skil(Shield) unlocked.\nPress <Q>.";
                break;
            case(4):
                noticeText = "Skill(WhirWind) unlocked.\nPress <E>.";
                break;
            case(5):
                noticeText = "Skill(Vampire) unlocked.\nPress <R>.";
                break;
            case(10):
                noticeText = "Need shovels to use skill.";
                break;
        }
        
        text.GetComponent<Text>().text = noticeText;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return new WaitForSecondsRealtime(3);

        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

    }
}
