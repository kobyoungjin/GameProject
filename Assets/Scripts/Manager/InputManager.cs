using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputManager : MonoBehaviour
{
    private bool moveInput;
    private bool attackInput;
    private bool keyCodeQ;
    private bool quitInput;
    private bool interactionInput;

    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool pressed = false;
    float pressedTime = 0;

    public bool MoveInput { get => moveInput; }
    public bool AttackInput { get => attackInput; }
    public bool KeyCodeQ { get => keyCodeQ; }
    public bool QuitInput { get => quitInput; }
    public bool InteractionInput { get => interactionInput; }

    GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        moveInput = gameManager.isAction ? false : Input.GetMouseButton(1);
        attackInput = gameManager.isAction ? false : Input.GetMouseButton(0);
        keyCodeQ = gameManager.isAction ? false : Input.GetKeyDown(KeyCode.Q);
        quitInput = Input.GetKeyDown(KeyCode.Escape);
        interactionInput = gameManager.isAction ? false : Input.GetKeyDown(KeyCode.G);

        //print("InputManager print"+moveInput);
        //if (Input.GetKeyDown(keyCode))
        //    keyCode = 
        //Debug.Log(pressed);
    }
    
    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(1)) // PointerDown -> Press
            {
                if (!pressed) // �ѹ��� ���������� �����µ� Ŭ�� ���°Ÿ� 
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    pressedTime = Time.time; // start ���� ��� �ð� (���и� �� �� ������ ��)
                }
                MouseAction.Invoke(Define.MouseEvent.Press); // ��ȿ������ �� ����(�̺�Ʈ �߻�)
                pressed = true;
            }
            else // Click(�ݹ� ���� ��) -> PointerUp(�� ���� �����ִٰ� ���� ��)
            {
                if (pressed)  // Ŭ�� �̺�Ʈ�� �ȵ��Դµ� ���� ���¿����ٸ�
                {
                    if (Time.time < pressedTime + 0.2f) // 0.2�� ���� ������
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                pressed = false;
                pressedTime = 0;
            }
        }
    }
    
    public void Clear()
    {
        MouseAction = null;
    }

    public bool GetIsPress()
    {
        return pressed;
    }
}

