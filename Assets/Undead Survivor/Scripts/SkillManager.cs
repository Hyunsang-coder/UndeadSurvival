using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public event Action<PlayerSkill> skillUpdate;
    public enum PlayerSkill{
        Dash, WirlWind, HolyShiled, VampireSpirit
    }

   PlayerSkill[] skills;

   public GameObject dashUI;

   public GameObject whirlUI;
   public GameObject shieldUI;
   public GameObject vampireUI;

   private float spacing = 12f;
    /*
    bool learnedDash;
    bool learnedWhirlWind;
    bool learnedHolyShield;
    bool learnedVampireSpirit;
    */

    private void Awake() {
        Instance = this;
        //skills = (PlayerSkill[]) System.Enum.GetValues(typeof(PlayerSkill));

        RepositionActiveChildren();
    }
    public void LearnSkill(PlayerSkill skill)
    {   
        switch(skill)
        {
            case(PlayerSkill.Dash):
                dashUI.gameObject.SetActive(true);
                NoticeManager.Instance.Notify(2);
                break;
            case(PlayerSkill.WirlWind):
                whirlUI.gameObject.SetActive(true);
                break;
            case(PlayerSkill.HolyShiled):
                shieldUI.gameObject.SetActive(true);
                break;
            case(PlayerSkill.VampireSpirit):
                vampireUI.gameObject.SetActive(true);
                break;    
        }
        
        skillUpdate?.Invoke(skill);
        RepositionActiveChildren();


    }

     void RepositionActiveChildren()
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
