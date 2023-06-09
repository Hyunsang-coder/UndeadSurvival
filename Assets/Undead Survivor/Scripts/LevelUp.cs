using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
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

        foreach(Item item in items)
        {
            if(item.gameObject.activeSelf){
                item.UpdateItemInfo();
            }
        }
    }

    public void Hide()
    {
        GameManager.Instance.ResumeGame();
        rect.localScale = Vector3.zero;
        
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }
}
