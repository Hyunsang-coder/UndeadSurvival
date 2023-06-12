using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public Vector2 InputVector { get; private set; }
    Rigidbody2D rigid;
    SpriteRenderer spriteRender;
    Animator anim;
    public Scanner scanner;
    public Hand[] hands; 

    const float origianlShootingSpeed = 0.3f;
    public float shootingSpeed;


    const float originalMoveSpeed = 3f; 
    public float moveSpeed;

    
    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        // include inactive components 
        hands = GetComponentsInChildren<Hand>(true);
    }
    void Start()
    {
        shootingSpeed = origianlShootingSpeed;
        moveSpeed = originalMoveSpeed;
    }

    //여기서 InputValue는 인풋시스템에서 자동으로 넘겨 줌
    void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }

    public void IncreaseShootingSpeed(float percentage){
        shootingSpeed = origianlShootingSpeed - (origianlShootingSpeed*percentage);
    }

    public void IncreaseMovementSpeed(float percentage){
        moveSpeed = originalMoveSpeed + (originalMoveSpeed*percentage);
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

        if (GameManager.Instance.health <= 0){
            for (int index=2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
