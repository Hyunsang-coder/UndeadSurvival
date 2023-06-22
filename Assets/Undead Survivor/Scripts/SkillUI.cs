using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillUI : MonoBehaviour
{
    public static SkillUI Instance;

    public enum PlayerSkill{
        Dash, WirlWind, HolyShield, VampireSpirit
    }

    public GameObject dashUI;
    public GameObject shieldUI;
    public GameObject whirlUI;
    public GameObject vampireUI;

    private float spacing = 15f;
    

    private void Awake() {
        Instance = this;
        
        RepositionSkillIndicators();
    }
    public void LearnSkill(PlayerSkill skill)
    {   
        switch(skill)
        {
            case(PlayerSkill.Dash):
                dashUI.gameObject.SetActive(true);
                NoticeSystem.Instance.Notify(2);
                break;
            
            case(PlayerSkill.HolyShield):
                shieldUI.gameObject.SetActive(true);
                NoticeSystem.Instance.Notify(3);
                break;

            case (PlayerSkill.WirlWind):
                whirlUI.gameObject.SetActive(true);
                break;

            case (PlayerSkill.VampireSpirit):
                vampireUI.gameObject.SetActive(true);
                break;    
        }
        
        //skillUpdate?.Invoke(skill);
        RepositionSkillIndicators();


    }

     void RepositionSkillIndicators()
    {
        List<Transform> activeChildren = new List<Transform>();

        // Find active children
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                activeChildren.Add(child);
            }
        }

        // If there's only one active child, center it
        if (activeChildren.Count == 1)
        {
            activeChildren[0].localPosition = Vector3.zero;
        }
        // If there's more than one, space them evenly
        else if (activeChildren.Count > 1)
        {
            // Calculate total width
            float totalWidth = spacing * (activeChildren.Count - 1);

            // Start from the leftmost position
            float currentX = -totalWidth / 2;

            for (int i = 0; i < activeChildren.Count; i++)
            {
                // Set the child's position
                activeChildren[i].localPosition = new Vector3(currentX, 0, 0);

                // Move to the next position
                currentX += spacing;
            }
        }
    }
}
