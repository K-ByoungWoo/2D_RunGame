using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float jumpPower = 8f;

    float actWait = 0;              //�ִϸ��̼��� �����ϰ� �ٷ� �ٲ��� �ʰ� �ϱ� ���� �ִϸ��̼� ���� �ð��� �����ϱ� ���� ����
    bool dbJump = false;            //���� ���� Ȯ�� ����
    bool isGround = false;          //���� ������ �ߴ��� Ȯ���ϴ� ����

    enum _state                     //�ִϸ��̼� ����
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
        bool JumpBtn = Input.GetButtonDown("Jump");     //Jump��ư ���� ����

        switch(st)
        {
            case _state._Run:
                {
                    isGround = true;                    //�޸��� ���¿����� ���� ���˵� �����Ƿ� true���� ����

                    if(isGround && JumpBtn)             //���� ������ �վ��� ������ư�� ���������� ��ȣ���� �ڵ带 ����
                    {
                        Jump();                         //���� ����
                        dbJump = true;                  //���� ���˵��־��� �����̹Ƿ� ���������� �� �� ������ true���� ����
                        isGround = false;               //������ �ѹ� �߱⶧���� ���� �ƴϹǷ� false���� ����
                        st = _state._Jump_Wait;         //�ִϸ��̼� ���� ����
                        anim.SetTrigger("jump");        //jump �ִϸ��̼����� ����
                        
                    }
                }break;

            case _state._Jump_Wait:
                {
                    if(dbJump && JumpBtn)               //���������� �� ���ְ� ������ �ѹ� �� �������� ��ȣ���� �ڵ带 ����
                    {
                        Jump();
                        dbJump = false;                 //���� ������ ���� �����Ƿ� false���� ����
                        anim.SetTrigger("jump");        //���� ���� state�� �̵�

                    }
                        actWait += Time.deltaTime;      
                        if(actWait > 0.5f)              //���� �ð� ������ 0.5f�� �ʰ��ϸ� 0���� �ʱ�ȭ �ϰ� �ִϸ��̼� ���¸� Jump�� ����
                        {
                            actWait = 0;
                            st = _state._Jump;
                        }
                }
                break;

            case _state._Jump:
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);   //Player�� ������ġ���� �Ʒ��������� ������ �Ÿ���ŭ
                    Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.black);            //�������� ���� �浹�� �����Ǹ� true���� �ݳ�

                    if (hit)                           //����ĳ��Ʈ ���� hit�� true���� �ݳ����� �� ��ȣ���� �ڵ带 ����
                    {
                        st = _state._Run;              //�ִϸ��̼� ���� ����
                        anim.SetTrigger("run");        //run �ִϸ��̼����� ����
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
        rb.velocity = Vector2.zero;                             //Player��ü�� �ӷ�(�ӵ��� ��ġ)���� (0,0)���� ����

        Vector2 jumpVelocity = new Vector2(0, jumpPower);       //Vector2���� jumpVelocity�� �����ϰ� y�ప�� jumpPower�� ����
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);         //Player��ü�� y��������� 8��ŭ�� ���� ���������� ����(����O)
    }
}
