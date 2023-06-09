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
    Collider2D collid;
    SpriteRenderer spriter;
    float knockBackForce = 3.4f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collid = GetComponent<Collider2D>();
        
    }

    private void OnEnable()
    {
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;


        collid.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder++;
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.isGameLive) return;

        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        // NextVec과 velocity가 충돌하지 않도록하기위해.
        rigid.velocity = Vector2.zero;

        
    }

    private void LateUpdate()
    {
        if(!GameManager.Instance.isGameLive) return;
        
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
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().GetDamage();

        StartCoroutine(KnockBack());

        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            collid.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder--;
            animator.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GainXP();
        }
    }

    IEnumerator KnockBack()
    {
        yield return new WaitForFixedUpdate();

        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 dirVec = (transform.position-playerPos).normalized;

        rigid.AddForce(dirVec * knockBackForce, ForceMode2D.Impulse);

    }

    public void Dead()
    {

        gameObject.SetActive(false);
    }
}
