using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "�����.",
                                            "��." });

        talkData.Add(10 + 1000, new string[] { "����������.", 
                                                "������������." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        
        if (!talkData.ContainsKey(id))
        {
            if(!talkData.ContainsKey(id - id % 10))
            {   // ����Ʈ �� ó�� ��縶�� ���� ��.
                // �⺻ ��縦 ������ �´�.
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {   // �ش� ����Ʈ ���� ���� ��簡 ���� ��.
                // ����Ʈ �� ó�� ��縦 ������ �´�.
                return GetTalk(id - id % 10, talkIndex);
            }
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
