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
    private bool isMove;

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

        if (inputManager.MoveInput)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out click) )  // Ŭ���� ���� �����ɽ�Ʈ
            {
                Debug.Log(click.point);
                SetPosition(click.point);
            }
        }

        Move();

        if(inputManager.AttackInput)
        {
            Attack();
        }
        else
        {
            animator.SetBool("isClicking", false);
        }
    }

    // �÷��̾� �̵� �Լ�
    private void Move()
    {
        if (Vector3.Distance(destination, this.transform.position) <= 0.1f)  // �Ÿ��� 0.1 ���� ������ ����
        {
            animator.SetBool("isMove", false);
            Debug.Log(isMove);
            return;
        }
        
        Vector3 pos = destination - this.transform.position;
        pos.y = 0f;

        var rotate = Quaternion.LookRotation(pos, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 5f);  // õõ�� ȸ��
        transform.position += pos.normalized * Time.deltaTime * speed;
    }

    // ���콺 ��ǥ �����Լ�
    void SetPosition(Vector3 pos)
    {
        destination = pos;
        animator.SetBool("isMove", true);
        Debug.Log(isMove);
    }

    // ���� �Լ�
    void Attack()  
    {
        animator.SetTrigger("isAttack");
        animator.SetBool("isClicking", true);
    }
}
