using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        
    }

    private void FixedUpdate()
    {
        if (!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        // NextVec과 충돌하지 않기 위
        rigid.velocity = Vector2.zero;

        
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
