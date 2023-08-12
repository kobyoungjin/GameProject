using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    Status status;
    public virtual void Init()
    {
        Bind<GameObject>(typeof(GameObject));
        status = transform.parent.GetComponent<Status>();
    }
    void Start()
    {
        Init();

        Debug.Log(transform.parent.name);
    }

    void Update()
    {
        Transform parent = transform.parent;
        if (transform.parent.name == "Troll_model")
            transform.position = parent.position + Vector3.up * (transform.parent.GetChild(2).GetComponent<Collider>().bounds.size.y);
        else
            transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;  // ������

        float ratio = status.Hp / (float)status.MaxHp;
        SetHpRatio(ratio);  // �����̴� �� �� �����Ӹ��� ����
    }

    public void SetHpRatio(float ratio)
    {
        //GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
