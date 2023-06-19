using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoticeManager : MonoBehaviour
{
    public static NoticeManager Instance;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Notify(int index)
    {
        StartCoroutine(ActivateNotice(index));
        Debug.Log("notify requested");
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
                noticeText = "Congrats!\nMan unlocked.";
                break;
            case(1):
                noticeText = "Congrats!\nWoman unlocked.";
                break;
            case(2):
                noticeText = "Dash unlocked.\nPress Space.";
                break;
            case(3):
                noticeText = "Congrats!\nWhirlWind unlocked.";
                break;
        }
        
        text.GetComponent<Text>().text = noticeText;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return new WaitForSecondsRealtime(3);

        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

    }
}
