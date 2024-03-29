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

    public bool hasShovel;
    public float shootingTimer = 0.3f;

    public float moveSpeed = 3f;

    bool isHit;
    float hitEffectTimer ;
    float hitEffectMaxTime = 0.3f;

    PlayerControl inputAction;

    [SerializeField] GameObject shield;

    [SerializeField] Weapon shovel;
    

    [Header("PlayerSkills")]
    public bool dashLearned;
    public bool whirlWindLearned;
    public bool holyShieldLarned;
    public bool vampireLearned;


    bool isDashing;
    public float dashForce = 10f;
    float dashDuration = 0.3f;
    public float dashTimer = 0f;
    public float dashCoolTime = 7f;
    
    bool isShielding;
    public float shieldDuration = 5f;
    public float shieldTimer = 0f;
    public float shieldCoolTime = 10f;

    bool isWhirWind;
    public float whirWindDuration = 5f;
    public float whirWindTimer = 0f;
    public float whirWindCoolTime = 10f;


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

        inputAction.Player.WhirlWind.performed += WhirlWind;
        inputAction.Player.WhirlWind.Enable();


        AchievementTracker.Instance.onLearningSkill += UpdatePlayerSkill;

        shield = transform.Find("Shield").gameObject;
        
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
        whirWindTimer += Time.deltaTime;
        
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

    void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>().normalized;
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

     void UpdatePlayerSkill(PlayerSkill skill){
        
        switch(skill)
        {
            case(PlayerSkill.Dash):
                dashLearned = true;
                dashTimer = dashCoolTime;
                break;
            
            case(PlayerSkill.Shield):
                holyShieldLarned = true;
                shieldTimer = shieldCoolTime;
                break;
            case (PlayerSkill.WhirlWind):
                whirlWindLearned = true;
                whirWindTimer = whirWindCoolTime;
                break;
            case (PlayerSkill.Vampire):
                vampireLearned = true;
                break;    
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

/*
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

    public float GetWhirWindTimeIndicator()
    {
        if (whirWindTimer < whirWindCoolTime)
        {
            return whirWindTimer / whirWindCoolTime;
        }
        else return 1f;
    }

    */

    public float GetSkillTimeIndicator(PlayerSkill skill)
    {
        switch(skill)
        {
            case(PlayerSkill.Dash):
                if (dashTimer < dashCoolTime)
                {
                    return dashTimer / dashCoolTime;
                }
                else return 1f;
            case(PlayerSkill.Shield):
                 if (shieldTimer < shieldCoolTime)
                {
                    return shieldTimer / shieldCoolTime;
                }
                else return 1f;
            case(PlayerSkill.WhirlWind):
                if (whirWindTimer < whirWindCoolTime)
                {
                    return whirWindTimer / whirWindCoolTime;
                }
                else return 1f;
            case(PlayerSkill.Vampire):
                break;
            default:
                break;
        }

        return 1f;
    }

    void UseShield(InputAction.CallbackContext context)
    {
        if (!holyShieldLarned || isShielding || shieldTimer < shieldCoolTime) return;
        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        isShielding = true;
        shieldTimer = 0;

        if(shield)
        {
            shield.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Shield object is empty!");
        }
        
        yield return new WaitForSeconds(shieldDuration);
        
        if(shield)
        {
            shield.gameObject.SetActive(false);
        }

        isShielding = false;

    }

    void WhirlWind(InputAction.CallbackContext context)
    {
        if (!whirlWindLearned || isWhirWind || whirWindTimer < whirWindCoolTime || !hasShovel) return;
        shovel = transform.Find("Weapon0").GetComponent<Weapon>();
        if (shovel == null ) return;

        StartCoroutine(WhirWindCoroutine());
    }

    IEnumerator WhirWindCoroutine()
    {
        isWhirWind = true;
        whirWindTimer = 0;
        shovel.FasterRotation(2f);

        if (shovel == null)
        {
            NoticeSystem.Instance.Notify(11);
        }

        yield return new WaitForSeconds(whirWindDuration);
        shovel.FasterRotation(0.5f);

        isWhirWind = false;
    }


}
