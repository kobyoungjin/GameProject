using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera camera;
    InputManager inputManager;
    private Animator animator;
    private RaycastHit click;

    Vector3 destination;

    private bool isAttack = false;
    private bool cannotMove = false;

    float speed = 4.0f;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        if (inputManager.MoveInput && !isAttack)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out click) )  // Ŭ���� ���� �����ɽ�Ʈ
            {
                Debug.Log(click.point);
                SetPosition(click.point);
            }
        }

        if (inputManager.AttackInput && !isAttack)
        {
            Attack();
            return;
        }

        if (inputManager.KeyCodeQ && !isAttack)
        {
            AbilityAttack();
            return;
        }

        Move();
    }

    // �÷��̾� �̵� �Լ�
    private void Move()
    {        
        SetAttackEnd();
        animator.SetBool("isAttack", false);
        animator.SetBool("isAbilityAttack", false);

        if (Vector3.Distance(destination, this.transform.position) <= 0.1f)  // �Ÿ��� 0.1 ���� ������ ����
        {
            Stop();
            
            return;
        }

        animator.SetBool("running", true);

        Vector3 pos = destination - this.transform.position;
        pos.y = 0f;

        var rotate = Quaternion.LookRotation(pos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 5f);  // õõ�� ȸ��
        transform.position += pos.normalized * Time.deltaTime * speed;

        return;
    }

    // ���� �Լ�
    private void Stop()
    {
        animator.SetBool("batIdle", true);

        if(!animator.GetBool("running"))
            animator.SetBool("batIdle", false);

        animator.SetBool("running", false);

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")    // �ִϸ������� State attackã�� 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)  // �ִϸ��̼� ���������� ��ٸ���
        {
            animator.SetBool("batIdle", true);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ability")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            animator.SetBool("batIdle", true);
        }

        destination = transform.position;
    }

    // ���콺 ��ǥ �����Լ�
    void SetPosition(Vector3 pos)
    {
        destination = pos;
    }

    // �Ϲ� ���� �Լ�
    void Attack()  
    {
        SetAttackStart();
        animator.SetBool("isAttack", true);
        Stop();
    }
    
    // ��ų �����Լ�
    void AbilityAttack()
    {
        SetAttackStart();
        animator.SetBool("isAbilityAttack", true);
        Stop();
    }

    // ���� ���� �����Լ�
    private void SetAttackStart()
    {
        isAttack = true;
        cannotMove = true;
    }

    // ���� �� �����Լ�
    private void SetAttackEnd()
    {
        isAttack = false;
        cannotMove = false;
    }
}
