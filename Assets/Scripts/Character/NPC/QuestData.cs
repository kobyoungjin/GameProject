using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName; //����Ʈ ��
    public int[] npcId; // ����Ʈ�� ���õ� NPC id ����


    public QuestData() { }

    //������
    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}