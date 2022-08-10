using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float jumpPower = 8f;

    float actWait = 0;              //애니메이션이 동작하고 바로 바뀌지 않게 하기 위해 애니메이션 동작 시간을 설정하기 위한 변수
    bool dbJump = false;            //더블 점프 확인 변수
    bool isGround = false;          //땅에 접촉을 했는지 확인하는 변수

    enum _state                     //애니메이션 상태
    {
        _Run,
        _Jump,
        _Jump_Wait,
        _Double_Jump,
        _Die
    }

    _state st = _state._Run;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool JumpBtn = Input.GetButtonDown("Jump");     //Jump버튼 동작 반응

        switch(st)
        {
            case _state._Run:
                {
                    isGround = true;                    //달리는 상태에서는 땅에 접촉되 있으므로 true값을 저장

                    if(isGround && JumpBtn)             //땅에 접촉해 잇었고 점프버튼이 눌러졌으면 괄호안의 코드를 실행
                    {
                        Jump();                         //점프 실행
                        dbJump = true;                  //땅에 접촉되있었던 상태이므로 더블점프를 할 수 있으니 true값을 저장
                        isGround = false;               //점프를 한번 했기때문에 땅이 아니므로 false값을 저장
                        st = _state._Jump_Wait;         //애니메이션 상태 변경
                        anim.SetTrigger("jump");        //jump 애니메이션으로 변경
                        
                    }
                }break;

            case _state._Jump_Wait:
                {
                    if(dbJump && JumpBtn)               //더블점프를 할 수있고 점프를 한번 더 눌렀으면 괄호안의 코드를 실행
                    {
                        Jump();
                        dbJump = false;                 //더블 점프를 실행 했으므로 false값을 저장
                        anim.SetTrigger("jump");        //더블 점프 state로 이동

                    }
                        actWait += Time.deltaTime;      
                        if(actWait > 0.5f)              //동작 시간 변수가 0.5f를 초과하면 0으로 초기화 하고 애니메이션 상태를 Jump로 변경
                        {
                            actWait = 0;
                            st = _state._Jump;
                        }
                }
                break;

            case _state._Jump:
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);   //Player의 현재위치에서 아래방향으로 지정한 거리만큼
                    Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.black);            //레이저를 쏴서 충돌이 감지되면 true값을 반납

                    if (hit)                           //레이캐스트 변수 hit가 true값을 반납했을 시 괄호안의 코드를 실행
                    {
                        st = _state._Run;              //애니메이션 상태 변경
                        anim.SetTrigger("run");        //run 애니메이션으로 변경
                    }
                }
                break;
                
            case _state._Die:
                {

                }break;
        }
    }

    void Jump()
    {
        anim.SetTrigger("jump");
        rb.velocity = Vector2.zero;                             //Player객체의 속력(속도와 위치)값을 (0,0)으로 설정

        Vector2 jumpVelocity = new Vector2(0, jumpPower);       //Vector2형의 jumpVelocity를 선언하고 y축값에 jumpPower를 저장
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);         //Player객체를 y축방향으로 8만큼의 힘을 순간적으로 가함(질량O)
    }
}
