using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePicking : MonoBehaviour
{
    InputManager inputManager;
    NPCDialogue npcDialogue;
    Renderer renderers;
    RaycastHit hit;
    Transform selected;
    NPC npc;
    bool result;
    bool isSelected = false;
    private void Start()
    {
        npc = GameObject.FindObjectOfType<NPC>().GetComponent<NPC>();
        //npcDialogue = GameObject.FindObjectOfType<NPCDialogue>().GetComponent<NPCDialogue>();
    }

    void Update()
    {
        result = Raycast();
        if (!result)
        {
            Debug.Log("MousePicking Update ����");
            return;
        }

        Debug.Log(isSelected);
    }

    bool Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
        int layer = 1 << LayerMask.NameToLayer("NPC");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Transform obj = hit.transform;

            if (isSelected)
            {
                isSelected = false;
                result = ClearTarget();
                if (!result)
                {
                    Debug.Log("Ÿ���� �������� ���߽��ϴ�.");
                    return false;
                }
            }

            result = SelectTarget(obj);
            if (!result)
            {
                Debug.Log("����� �������� ���߽��ϴ�.");
                return false;
            }
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }

        return true;
    }

    // outline �������ִ� �Լ�
    bool AddOutline(Transform obj)
    {
        if (obj == null)
        {
            Debug.Log("obj error");
            return false;
        }

        renderers = obj.GetComponent<Renderer>();
        renderers.sharedMaterial.SetFloat("_OutLineWidth", 0.03f);
        return true;
    }

    // outline Ǯ���ִ� �Լ�
    bool RemoveOutline(Renderer renderer)
    {
        if (renderer == null)
        {
            Debug.Log("renderer�� ����� �����ϴ�");
            return false;
        }

        renderer.sharedMaterial.SetFloat("_OutLineWidth", 0.001f);

        return true;
    }

    // Ÿ������
    public bool ClearTarget()
    {
        if (selected == null)
        {
            Debug.Log("������ Ÿ���� �����ϴ�.");
            return false;
        }

        result = RemoveOutline(renderers);
        if (!result)
        {
            Debug.Log("���� �ƿ������� �������� ���߽��ϴ�.");
            return false;
        }
        selected = null;

        return true;
    }

    // Ÿ�� ����
    bool SelectTarget(Transform obj)
    {
        if (obj == null)
        {
            Debug.Log("obj error");
            return false;
        }
        
        selected = obj;

        result = AddOutline(obj);
        if (!result)
        {
            Debug.Log("����� �������� ���߽��ϴ�");
            return false;
        }


        return true;
    }

}
