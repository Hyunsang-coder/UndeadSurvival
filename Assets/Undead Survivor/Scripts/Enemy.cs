using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // RuntimeAnimatorControllers는 컴포넌트가 아님!
    [SerializeField] RuntimeAnimatorController[] runTimeAC;

    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Animator animator;


    [SerializeField] Rigidbody2D target;
    bool isLive;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }

    private void OnEnable()
    {
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        // NextVec과 velocity가 충돌하지 않도록하기위해.
        rigid.velocity = Vector2.zero;

        
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }
    
    public void EnemyInint(SpawnData data)
    {
        maxHealth = data.maxHealth;
        health = data.maxHealth;
        speed = data.speed;

        animator.runtimeAnimatorController = runTimeAC[data.enemyType];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().GetDamage();
            Debug.Log("Collided"); 
        }

        if (health > 0)
        {
            //
        }
        else
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
