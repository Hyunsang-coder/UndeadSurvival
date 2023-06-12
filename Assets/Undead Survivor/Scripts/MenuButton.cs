using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    public enum CharacterType{
        Character0,
        Character1,
        Character2,
        Character3
    }
    Button button;
    GameObject startMenuObject;
    GameObject HUD;

    public CharacterType characterType;

    
    void Awake()
    {
        button = GetComponent<Button>();
        startMenuObject = transform.parent.gameObject.transform.parent.gameObject;
        HUD = FindObjectOfType<HUD>(true).transform.parent.gameObject;

        switch(characterType)
        {
            case CharacterType.Character0:
            button.onClick.AddListener(()=>
            {
                startMenuObject.SetActive(false);
                HUD.SetActive(true);
                GameManager.Instance.GameStart(0);
            });
                break;

                
            case CharacterType.Character1:
            button.onClick.AddListener(()=>
            {
                startMenuObject.SetActive(false);
                HUD.SetActive(true);
                GameManager.Instance.GameStart(1);
            });
                break;
            case CharacterType.Character2:
            button.onClick.AddListener(()=>
            {
                startMenuObject.SetActive(false);
                HUD.SetActive(true);
                GameManager.Instance.GameStart(2);
            });
                break;
            case CharacterType.Character3:
            button.onClick.AddListener(()=>
            {
                startMenuObject.SetActive(false);
                HUD.SetActive(true);
                GameManager.Instance.GameStart(3);
            });
                break;

        }
        
        
    }

}
