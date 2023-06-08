using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // editor에서 체크 whether it's right hand or left hand
    public bool isLeftHand;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f);
    Vector3 reverseRightPose  = new Vector3(-0.15f, -0.15f, 0);

    Quaternion leftRotation = Quaternion.Euler(0, 0, -25);
    Quaternion reverseRightRotation = Quaternion.Euler(0, 0, -155);

    private void Awake() {
        // 0 means self sprite render

        spriter = GetComponent<SpriteRenderer>();
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate() {
        // whther it's fliped or not
        bool isReverse = player.flipX;

        if(isLeftHand){ // 근접무기
            transform.localRotation = isReverse? reverseRightRotation: leftRotation;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse? 4: 6;
        }
        else{ // 원거리 무기 
            transform.localPosition= isReverse? reverseRightPose: rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse? 6: 4;
        }
    }


}
