using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    Button button;
    public int level;
    public Weapon weapon;

    Image icon;
    Text levelTxt;
    Text nameTxt;
    Text descTxt;


    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
        button = GetComponent<Button>();

        Text[] texts = GetComponentsInChildren<Text>();
        levelTxt = texts[0];
        nameTxt = texts[1];
        descTxt = texts[2];
        nameTxt.text = data.itemName;

        

    }

    private void OnEnable() {
        levelTxt.text = "Lv." + (level+1);

        UpdateItemInfo();
        
    }

    public void UpdateItemInfo()
    {
        switch (data.itemType){
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                descTxt.text = string.Format(data.itemDesc, data.damages[level] *100, data.counts[level]);
                levelTxt.text = "Lv." + (level+1);
                break;
            
            case ItemData.ItemType.Glove:
                descTxt.text = string.Format(data.itemDesc, data.damages[level]*100);
                levelTxt.text = "Lv." + (level+1);
                break;

            case ItemData.ItemType.Shoe:
                descTxt.text = string.Format(data.itemDesc, data.damages[level] *100);
                levelTxt.text = "Lv." + (level+1);
                break;
            default:
                descTxt.text = string.Format(data.itemDesc);
                levelTxt.text = "Lv." + (level+1);
                break;

        }
    }

    private void Start() {
        button.onClick.AddListener(OnClick);
        
        /*
        button.onClick.AddListener(()=>{
            
            GameObject.FindObjectOfType<LevelUp>().Hide();
        });
        */
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();

                    weapon.Init(data);
                }
                else
                {
                    float nextDmage = data.baseDamage;
                    int nextCount = 0;

                    nextDmage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDmage, nextCount);
                }

                level++;

                if (level == data.damages.Length) 
                {
                    button.interactable = false;
                }

                break;
            case ItemData.ItemType.Shoe:
                break;
            case ItemData.ItemType.Glove:
                break;
            case ItemData.ItemType.Potion:
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }

        
        

        GameObject.FindObjectOfType<LevelUp>().Hide();
    }
}
