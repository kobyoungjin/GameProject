using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            //  �ش� Ÿ���� �̱��� ������Ʈ�� ã�� ���ϸ� �õ�
            instance = FindObjectOfType<T>();

            if (instance != null)
                return instance;

            //  �׷��� ������ ����
            CreateInstance();


            return instance;
        }

        private set { return; }  //  ���� �ܺο��� ���Ƿ� set ���� ����.
    }

    private static T CreateInstance()
    {
        GameObject Obj = new GameObject(typeof(T).ToString());  //  ��� Ŭ���� �̸����� ���ӿ�����Ʈ ����
        instance = Obj.AddComponent<T>();

        return instance;
    }

    protected virtual void Awake()
    {
        instance = FindObjectOfType<T>();
    }

    //  Ŭ���� �ı� �� ���⼭ �ı����� �ʰ� �ڽ� Ŭ������ ���� �ı��ڸ� ȣ���ϵ��� ����ȭ
    virtual protected void OnApplicationQuit() { }
    virtual protected void OnDestroy() { }
}
