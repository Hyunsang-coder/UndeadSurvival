using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{

    public event Action<float> OnShootingSpeedChanged;
    public Vector2 inputVector;
    Rigidbody2D rigid;
    SpriteRenderer spriteRender;
    Animator anim;
    public Scanner scanner;
    public Hand[] hands; 

    public RuntimeAnimatorController[] animControllers;

    public PlayerData[] playerData;

    
    public float shootingTimer = 0.3f;

    public float moveSpeed = 3f;

    bool isHit;
    float hitEffectTimer ;
    float hitEffectMaxTime = 0.3f;

    PlayerControl inputAction;
    

    [Header("PlayerSkills")]
    public bool dashLearned;
    public bool whirlWindLearned;
    public bool holyShieldLarned;
    public bool vampireSpiritLearned;


    bool isDashing;
    public float dashForce = 10f;
    float dashDuration = 0.3f;
    public float dashTimer = 0f;
    public float dashCoolTime = 7f;
    
    bool isShielding;
    float shieldDuration = 3f;
    public float shieldTimer = 0f;
    public float shieldCoolTime = 10f;


    private void OnEnable() {
        anim.runtimeAnimatorController = animControllers[GameManager.Instance.playerID];
        
        
        // input system code에 이벤트 등록
        inputAction = new PlayerControl();
        
        inputAction.Player.Move.performed += Move;
        inputAction.Player.Move.canceled += Move;
        inputAction.Player.Move.Enable();

        inputAction.Player.Dash.performed += Dash;
        inputAction.Player.Dash.Enable();

        inputAction.Player.Shield.performed += UseShield;
        inputAction.Player.Shield.Enable();

        SkillManager.Instance.skillUpdate += UpdatePlayerSkill;
        
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

    void UpdatePlayerSkill(SkillManager.PlayerSkill skill){
        
        switch(skill)
        {
            case(SkillManager.PlayerSkill.Dash):
                dashLearned = true;
                dashTimer = dashCoolTime;
                break;
            
            case(SkillManager.PlayerSkill.HolyShield):
                holyShieldLarned = true;
                shieldTimer = shieldCoolTime;
                break;
            case (SkillManager.PlayerSkill.WirlWind):
                whirlWindLearned = true;
                break;
            case (SkillManager.PlayerSkill.VampireSpirit):
                vampireSpiritLearned = true;
                break;    
        }
    }

    

    void Start()
    {
        shootingTimer = shootingTimer * playerData[GameManager.Instance.playerID].shootingSpeed;
        moveSpeed = moveSpeed * playerData[GameManager.Instance.playerID].moveSpeed;
        
    }

    
    private void Update() {
        hitEffectTimer +=Time.deltaTime;
        dashTimer += Time.deltaTime;
        shieldTimer += Time.deltaTime;
        
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
    void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>().normalized;
    }


    
    private void FixedUpdate()
    {
        if(!GameManager.Instance.isGameLive) return;

        if (isDashing) return;

        Vector2 nextPosition = inputVector * Time.fixedDeltaTime * moveSpeed;
        rigid.MovePosition(rigid.position + nextPosition );

        if (inputVector.x != 0)
        {
            spriteRender.flipX = inputVector.x < 0;
        }
        

        anim.SetFloat("Speed", inputVector.magnitude);


    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!GameManager.Instance.isGameLive) return;

        if (isShielding) return;

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


    
    void Dash(InputAction.CallbackContext context)
    {
        if (!dashLearned || isDashing || dashTimer < dashCoolTime) return;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {

        isDashing = true;
        dashTimer = 0;
        Vector2 dashDir;
        if (inputVector == Vector2.zero)
        {
            dashDir = spriteRender.flipX? Vector2.left: Vector2.right;
        }
        else
        {
            dashDir = inputVector;
        }
        
        // dash for 0.2 seconds
        for (float t = 0; t < dashDuration; t += Time.deltaTime)
        {
            transform.position += (Vector3)(dashDir * dashForce * Time.deltaTime);
            yield return null;  // wait until the next frame
        }

        isDashing = false;
    }

    public float GetDashCoolTimeIndicator()
    {
        if (dashTimer < dashCoolTime)
        {
            return dashTimer / dashCoolTime;
        }
        else return 1f;
    }

    public float GetShieldCoolTimeIndicator()
    {
        if (shieldTimer < shieldCoolTime)
        {
            return shieldTimer / shieldCoolTime;
        }
        else return 1f;
    }

    void UseShield(InputAction.CallbackContext context)
    {
        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        isShielding = true;
        shieldTimer = 0;
        yield return new WaitForSeconds(shieldDuration);

        isDashing = false;

    }


}
