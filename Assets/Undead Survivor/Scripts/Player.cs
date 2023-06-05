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

    [SerializeField] float moveSpeed = 3f;

    
    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    void Start()
    {
        
    }

    //여기서 InputValue는 인풋시스템에서 자동으로 넘겨 줌
    void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }



    private void FixedUpdate()
    {
        Vector2 nextPosition = InputVector * Time.fixedDeltaTime * moveSpeed;
        rigid.MovePosition(rigid.position + nextPosition );

        if (InputVector.x != 0)
        {
            spriteRender.flipX = InputVector.x < 0;
        }

        anim.SetFloat("Speed", InputVector.magnitude);


    }
}
