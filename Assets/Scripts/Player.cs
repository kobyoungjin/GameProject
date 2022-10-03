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
        animator.SetBool("batIdle", false);

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
    
        Move();
    }

    // �÷��̾� �̵� �Լ�
    private void Move()
    {
        SetAttackEnd();

        if (Vector3.Distance(destination, this.transform.position) <= 0.1f)  // �Ÿ��� 0.1 ���� ������ ����
        {
            Stop();
            
            return;
        }

        animator.SetBool("Running", true);

        Vector3 pos = destination - this.transform.position;
        pos.y = 0f;

        var rotate = Quaternion.LookRotation(pos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 5f);  // õõ�� ȸ��
        transform.position += pos.normalized * Time.deltaTime * speed;

        return;
    }

    private void Stop()
    {
        animator.SetBool("Running", false);

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") 
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
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

    // ���� �Լ�
    void Attack()  
    {
        SetAttackStart();

        Stop();
    }

    private void SetAttackStart()
    {
        isAttack = true;

        animator.SetBool("isAttack", true);
        animator.SetBool("batIdle", false);
        animator.SetBool("Running", false);
    }

    private void SetAttackEnd()
    {
        isAttack = false;

        animator.SetBool("isAttack", false);
    }
}
