using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    RectTransform rect;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate() {
        
        rect.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.transform.position) ;
    }
}
