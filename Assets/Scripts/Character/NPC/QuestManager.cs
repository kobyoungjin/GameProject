using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;

    private int trollKilled;
    private int maxKill = 3;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        //������ �̿� (string name, int[] npcid)
        questList.Add(10, new QuestData("��� ��ġ    " + trollKilled + " / " + maxKill,
                                        new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("������ ���� ó��",
                                        new int[] { 5000, 2000 }));

        questList.Add(30, new QuestData("����Ʈ �Ϸ�",
                                        new int[] { 0 }));
    }
    public int GetQuestTalkIndex(int id) // Npc Id�� �޾� ����Ʈ ��ȣ�� ��ȯ�ϴ� �Լ� 
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    
    public void KilledTroll()
    {
        trollKilled += 1;
    }
}
