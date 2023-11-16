using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;

    public int trollKilled = 0;
    private int maxKill = 3;

    GameObject questBody;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    private void Start()
    {
        questBody = GameObject.Find("EtcCanvas").transform.GetChild(3).GetChild(1).gameObject;
    }

    void GenerateData()
    {
        //������ �̿� (string name, int[] npcid)
        questList.Add(20, new QuestData("�ѽ��� ��ȭ�ϱ�",
                                        new int[] { 1000, 2000 }));

        questList.Add(30, new QuestData("Ʈ�� ��ġ    " + trollKilled + " / " + maxKill,
                                        new int[] { 2000, 3000 }));

        questList.Add(40, new QuestData("������ ���� óġ�ϱ�",
                                        new int[] { 1000, 3000 }));

        questList.Add(50, new QuestData("����Ʈ �Ϸ�",
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

    public void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    
    public void KilledTroll()
    {
        trollKilled += 1;
        if(questBody.transform.childCount != 0 && trollKilled < maxKill)
        {
            questBody.GetComponentInChildren<Text>().text = "Ʈ�� ��ġ    " + trollKilled + " / " + maxKill;
        }
    }
}
