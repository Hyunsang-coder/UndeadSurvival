using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    public enum ButtonType{
        StartButton,
        RetryButton,
    }
    Button button;
    GameObject parentObject;
    GameObject HUD;

    public ButtonType type;

    
    void OnEnable()
    {
        button = GetComponent<Button>();
        parentObject = transform.parent.gameObject;
        HUD = FindObjectOfType<HUD>(true).transform.parent.gameObject;

        switch(type)
        {
            case ButtonType.StartButton:
            button.onClick.AddListener(()=>
            {
                parentObject.SetActive(false);
                HUD.SetActive(true);
                GameManager.Instance.GameStart();
            });
                break;

                
            case ButtonType.RetryButton:

            button.onClick.AddListener(()=>
            {
               GameManager.Instance.RetryGame();
            });
                break;

        }
        
        
    }

}
