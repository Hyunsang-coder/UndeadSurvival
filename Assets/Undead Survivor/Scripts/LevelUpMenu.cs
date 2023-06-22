using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    RectTransform rect;
    public Item[] items;
    private void Awake() {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }
    
    public void Show()
    {
        GameManager.Instance.StopGame();

        
        rect.localScale = Vector3.one;

        Next();

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.Instance.ApplyAudioFilter(true);

        
    }

    public void Hide()
    {
        GameManager.Instance.ResumeGame();
        rect.localScale = Vector3.zero;

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.Instance.ApplyAudioFilter(false);
        
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }


        // 중복되지 않는 3개 랜덤 숫자
        int[] randomNum = new int[3];

        while(true){
            randomNum[0] = Random.Range(0, items.Length);
            randomNum[1] = Random.Range(0, items.Length);
            randomNum[2] = Random.Range(0, items.Length);


            if(randomNum[0]!= randomNum[1] && randomNum[1]!=randomNum[2] && randomNum[2]!= randomNum[0])
                break;
        }

        for (int index=0; index < randomNum.Length; index++)
        {
            Item randomItem = items[randomNum[index]];

            // 스킬이 만랩이면 소비아이템으로 변경 
            if(randomItem.level == randomItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else{
                randomItem.gameObject.SetActive(true);
            }
        }
    }
}
