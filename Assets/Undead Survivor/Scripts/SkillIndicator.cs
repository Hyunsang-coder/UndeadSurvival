using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIndicator : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float  coolTimeIndicator;
    [SerializeField] Image image;

    public SkillUI.PlayerSkill skill;

    void Start()
    {
        
    }

    private void OnEnable() {
        player =  GameManager.Instance.Player;    
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (skill)
        {
            case SkillUI.PlayerSkill.Dash:

                coolTimeIndicator = player.GetDashCoolTimeIndicator();
                break;

            case SkillUI.PlayerSkill.HolyShield:

                coolTimeIndicator = player.GetShieldCoolTimeIndicator();
                break;
        }
        

    }

    void LateUpdate() {

        image.fillAmount = 1- coolTimeIndicator;
    }
}
