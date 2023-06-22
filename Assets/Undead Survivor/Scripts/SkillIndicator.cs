using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIndicator : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float  coolTimeIndicator;
    [SerializeField] Image image;

    public PlayerSkill skill;

    private void OnEnable() {
        player =  GameManager.Instance.Player;    
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (skill)
        {
            case PlayerSkill.Dash:
                coolTimeIndicator = player.GetSkillTimeIndicator(PlayerSkill.Dash);
                break;

            case PlayerSkill.Shield:

                coolTimeIndicator = player.GetSkillTimeIndicator(PlayerSkill.Shield);
                break;
            case PlayerSkill.WhirlWind:
                coolTimeIndicator = player.GetSkillTimeIndicator(PlayerSkill.WhirlWind);
                break;
        }
        

    }

    void LateUpdate() {

        image.fillAmount = 1 - coolTimeIndicator;
    }
}
