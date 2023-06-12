using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Object/PlayerData")]
public class PlayerData : ScriptableObject
{
    public enum PlayerType{Boy, Girl}

    [Header("Bonus Ability")]
    public float damage;
    public float moveSpeed;

    public float shootingSpeed;
    public int pierceCount;
    public int weaponCount;


    [Header("Character Data")]
    public Sprite characterSprite;

}
