using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour//InheritSingleton<GameManager>
{
    public static bool IsLooping { set; get; } = true;


    //protected override void Awake()
    //{
    //    base.Awake();

    //    var objs = FindObjectsOfType<GameManager>();
    //    if (objs.Length == 1)  // GameManagerŸ���� ������ 1���϶��� 
    //        DontDestroyOnLoad(this.gameObject);
    //    else  // �ƴϸ� ����
    //        Destroy(this.gameObject);


    //    entity = FindObjectOfType<Player>().GetComponent<Player>();
    //    entity.Init(entity.name);

    //    return;
    //}

    void Start()
    {
     
    }

    void Update()
    {
       

        
        //entity.Updated();
    }

   
        
}
