using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float  coolTimeIndicator;
    [SerializeField] Image image;

    public SkillManager.PlayerSkill skill;

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
            case SkillManager.PlayerSkill.Dash:

                coolTimeIndicator = player.GetDashCoolTimeIndicator();
                break;

            case SkillManager.PlayerSkill.HolyShield:

                coolTimeIndicator = player.GetShieldCoolTimeIndicator();
                break;
        }
        

    }

    void LateUpdate() {

        image.fillAmount = 1- coolTimeIndicator;
    }
}
