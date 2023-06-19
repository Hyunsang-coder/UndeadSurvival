using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float  coolTimeIndicator;
    [SerializeField] Image image;
    
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
        if (player.dashTimer >= player.dashCoolTime)
        {
            coolTimeIndicator = 1f;
        }
        else if (player.dashTimer < player.dashCoolTime)
        {
            coolTimeIndicator = player.dashTimer / player.dashCoolTime;
        }

    }

    void LateUpdate() {
        image.fillAmount = 1- coolTimeIndicator;
    }
}
