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
        if (!mySlider) {Debug.Log("Failed to access mySlider");};
    }


    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:

                float currentXP = GameManager.Instance.XP;
                float maxXP = GameManager.Instance.NextLvXP[GameManager.Instance.Level];

                mySlider.value = currentXP/maxXP;
                break;

            case InfoType.Level:

                break;
            case InfoType.Kill:

                break;

            case InfoType.Time:

                break;

            case InfoType.Health:

                break;

        }
    }
}
