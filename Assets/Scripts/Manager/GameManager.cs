using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : InheritSingleton<GameManager>
{
    Player entity;

    protected override void Awake()
    {
        base.Awake();
        
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length == 1)  // GameManagerŸ���� ������ 1���϶��� 
            DontDestroyOnLoad(this.gameObject);
        else  // �ƴϸ� ����
            Destroy(this.gameObject);


        entity = FindObjectOfType<Player>().GetComponent<Player>();
        entity.Init(entity.name);

        return;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //entity.Updated();
    }
}
