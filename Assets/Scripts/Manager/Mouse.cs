using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType cursorType = CursorType.None;

    InputManager inputManager;
    NPCDialogue npcDialogue;
    Renderer renderers;
    Transform selectedTarget;
    RaycastHit hit;

    Texture2D attackIcon;
    Texture2D handIcon;

    private void Start()
    {
        //npcDialogue = GameObject.FindObjectOfType<NPCDialogue>().GetComponent<NPCDialogue>();
        attackIcon = Resources.Load<Texture2D>("Cursors/Cursor 64/Cursor_Attack");
        handIcon = Resources.Load<Texture2D>("Cursors/Cursor 64/Cursor_Basic2");
    }

    void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
        int layer = 1 << LayerMask.NameToLayer("NPC");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Transform obj = hit.transform;
            SelectTarget(obj);

            //if(inputManager.MoveInput)
            //{
            //    npcDialogue.SetDialogue(obj);
            //}

        }
        else
        {
            ClearTarget();
        }
    }

    // outline �������ִ� �Լ�
    void AddOutline(Transform obj)
    {
        if (obj == null) return;

        renderers = obj.GetComponent<Renderer>();
        renderers.sharedMaterial.SetFloat("_OutLineWidth", 0.03f);
    }

    // outline Ǯ���ִ� �Լ�
    void RemoveOutline(Renderer renderer)
    {
        if (renderer != null)
        {
            renderer.sharedMaterial.SetFloat("_OutLineWidth", 0.001f);
        }
    }

    // Ÿ������
    void ClearTarget()
    {
        if (selectedTarget == null) return;

        selectedTarget = null;
        RemoveOutline(renderers);
    }

    // Ÿ�� ����
    void SelectTarget(Transform obj)
    {
        if (obj == null) return;

        ClearTarget();
        selectedTarget = obj;
        AddOutline(obj);
    }

    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, 7))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 5, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 3, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }
            }
        }
    }
}
