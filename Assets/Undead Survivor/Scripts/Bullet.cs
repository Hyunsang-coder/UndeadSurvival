using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] int pierce;
    Rigidbody2D rigid;

    float timer;
    [SerializeField] float bulletLifetime = 3f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int pierce, Vector3 dir)
    {
        this.damage = damage;
        this.pierce = pierce;


        // -1 means it's a melee weapon
        if (pierce > -1)
        {
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || pierce == -100) return;

        pierce--;

        if (pierce < 0) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }

    public float GetDamage()
    {
        return damage;
    }


    private void Update()
    {
        if(!GameManager.Instance.isGameLive) return;
        
        if (pierce == -100) return;

        timer += Time.deltaTime;

        if (timer > bulletLifetime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }

    }
}
