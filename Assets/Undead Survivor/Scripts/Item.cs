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
    Text textLevel;

    private void Awake() {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];

        button = GetComponent<Button>();

    }

    private void Start() {
        button.onClick.AddListener(()=>
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
                    nextCount += data.baseCount + data.counts[level];

                    weapon.LevelUp(nextDmage, nextCount);
                }

                    break;
                case ItemData.ItemType.Shoe:
                    break;
                case ItemData.ItemType.Glove:
                    break;
                case ItemData.ItemType.Potion:
                    break;
            }

            level++;

            if (level == data.damages.Length) 
            {
                button.interactable = false;
            }
        });
    }

    private void LateUpdate() {
        
        textLevel.text = "Lv." + (level);

    }
}
