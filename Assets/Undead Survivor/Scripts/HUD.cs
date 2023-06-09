using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public enum InfoType {Exp, Level, Kill, Time, Health }

    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }


    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:

                float currentXP = GameManager.Instance.XP;
                float maxXP = GameManager.Instance.NextLvXP[GameManager.Instance.playerLevel];

                mySlider.value = currentXP/maxXP;
                break;

            case InfoType.Level:
            // F0은 소수점이 0, F1은 소수점 1자리
            myText.text = string.Format("Level.{0:F0}",GameManager.Instance.playerLevel + 1);

                break;
            case InfoType.Kill:
            myText.text = string.Format("{0:F0}",GameManager.Instance.kill);
                break;

            case InfoType.Time:
            float remainTime = GameManager.Instance.MaxGameTime - GameManager.Instance.GameTime;
            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
            myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Health:
            
                float currentHP = GameManager.Instance.helath;
                float maxHP = GameManager.Instance.maxHealth;

                mySlider.value = currentHP/maxHP;

                break;

        }
    }
}
