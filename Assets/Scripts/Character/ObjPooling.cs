using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPooling : MonoBehaviour
{
    //[SerializeField] private GameObject objectPrefeb;
    Queue<GameObject> ObjectPool = new Queue<GameObject>(); //������Ʈ�� ���� ť
    public static ObjPooling instance = null;

    [SerializeField] private GameObject m1;
    [SerializeField] private GameObject m2;
    [SerializeField] private GameObject m3;
    [SerializeField] private GameObject m4;
    [SerializeField] private GameObject m5;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            ObjectPool.Enqueue(m1);
            ObjectPool.Enqueue(m2);
            ObjectPool.Enqueue(m3);
            ObjectPool.Enqueue(m4);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    GameObject CreateObject() //�ʱ� OR ������Ʈ Ǯ�� ���� ������Ʈ�� ������ ��, ������Ʈ�� �����ϱ����� ȣ��Ǵ� �Լ�
    {
        GameObject newObj = Instantiate(m1, instance.transform);
        newObj.gameObject.SetActive(false);

        return newObj;
    }
    public GameObject GetObject() //������Ʈ�� �ʿ��� �� �ٸ� ��ũ��Ʈ���� ȣ��Ǵ� �Լ�
    {
        if (ObjectPool.Count > 0) //���� ť�� �����ִ� ������Ʈ�� �ִٸ�,
        {
            GameObject objectInPool = ObjectPool.Dequeue();

            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
        else //ť�� �����ִ� ������Ʈ�� ���� �� ���� ���� ���
        {
            GameObject objectInPool = CreateObject();

            objectInPool.gameObject.SetActive(true);
            objectInPool.transform.SetParent(null);
            return objectInPool;
        }
    }
    public void ReturnObjectToQueue(GameObject obj) //����� �Ϸ� �� ������Ʈ�� �ٽ� ť�� ������ ȣ�� �Ķ����->��Ȱ��ȭ �� ������Ʈ
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.ObjectPool.Enqueue(obj); //�ٽ� ť�� ����
    }
}
