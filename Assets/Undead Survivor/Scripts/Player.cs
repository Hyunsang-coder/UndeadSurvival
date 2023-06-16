using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{

    public event Action<float> OnShootingSpeedChanged;
    [SerializeField] public Vector2 InputVector { get; private set; }
    Rigidbody2D rigid;
    SpriteRenderer spriteRender;
    Animator anim;
    public Scanner scanner;
    public Hand[] hands; 

    public RuntimeAnimatorController[] animControllers;

    public PlayerData[] playerData;

    
    public float shootingTimer = 0.3f;

    public float moveSpeed = 3f;

    float hitEffectTimer ;
    float hitEffectMaxTime = 0.3f;


    private void OnEnable() {
        anim.runtimeAnimatorController = animControllers[GameManager.Instance.playerID];
    }
    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        // include inactive components 
        hands = GetComponentsInChildren<Hand>(true);

        hitEffectTimer = hitEffectMaxTime;
    }
    void Start()
    {
        shootingTimer = shootingTimer * playerData[GameManager.Instance.playerID].shootingSpeed;
        moveSpeed = moveSpeed * playerData[GameManager.Instance.playerID].moveSpeed;
        
    }

    bool isHit;
    private void Update() {
        hitEffectTimer +=Time.deltaTime;
        
        isHit = (hitEffectTimer < hitEffectMaxTime)? true: false;

        if (isHit)
        {
            spriteRender.color = Color.red;
        }
        else 
        {
            spriteRender.color = Color.white;
        }
        
    }

    //여기서 InputValue는 인풋시스템에서 자동으로 넘겨 줌
    void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }



    private void FixedUpdate()
    {
        if(!GameManager.Instance.isGameLive) return;

        Vector2 nextPosition = InputVector * Time.fixedDeltaTime * moveSpeed;
        rigid.MovePosition(rigid.position + nextPosition );

        if (InputVector.x != 0)
        {
            spriteRender.flipX = InputVector.x < 0;
        }

        anim.SetFloat("Speed", InputVector.magnitude);


    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!GameManager.Instance.isGameLive) return;

        GameManager.Instance.health -= Time.deltaTime * 10;
        hitEffectTimer = 0;


        if (GameManager.Instance.health <= 0){
            for (int index=2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // 사망 시 다시 흰색으로
            spriteRender.color = Color.white;

            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }

    
    public void MoveSpeedUp(float percentage)
    {
        moveSpeed = moveSpeed + (moveSpeed *percentage);
    }

    public void ShootSpeedUp(float percetnage)
    {
        shootingTimer = shootingTimer - (shootingTimer * percetnage);
        OnShootingSpeedChanged.Invoke(shootingTimer);
    }

    void PlayerHitEffect()
    {
        return;
    }
}
