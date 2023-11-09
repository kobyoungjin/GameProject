using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    Dictionary<int, QuestData> questList;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        //������ �̿� (string name, int[] npcid)
        questList.Add(10, new QuestData("��� ��ġ", new int[] { 1000, 2000 }));
    }
    public int GetQuestTalkIndex(int id) // Npc Id�� �޾� ����Ʈ ��ȣ�� ��ȯ�ϴ� �Լ� 
    {
        return questId;
    }
}
