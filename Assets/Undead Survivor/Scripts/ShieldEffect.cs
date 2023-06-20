using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    Material material;
    SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }
    void Start()
    {
        
    }

    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1 )timer = 0;

        material.SetFloat("_Progress", timer);
    }
}
