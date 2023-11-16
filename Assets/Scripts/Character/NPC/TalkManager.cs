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
        talkData.Add(1000, new string[] { "����� ���� �츣���Դϴ�."
                                            });

        talkData.Add(10 + 1000, new string[] { "�߿��̽��ϴ�. ���谡�� :0",
                                            "���� �� �������� ������ �ϰ� �ִ� ��Ƽ���̶�� �մϴٸ�. :1",
                                            "�ֱ� ���� ��ó�� �ִ� �������� ���� �ֹε��� �� �� ���� �Ҹ��� ���� �鸰�ٰ� �ϳ�. :2",
                                            "�ڼ��� �̾߱�� ������ �ѽ��� �ڼ��� �������ٰɼ�. :3"});

        talkData.Add(20 + 2000, new string[] { "�ȳ��Ͻʴϱ� ���谡��! :0",
                                            "��Ϸ� ã�ƿ��̳���?",
                                            "������ �����Ϸ� ã�ƿ��̱���. :1",
                                            "���� ��ó�� �ֱ�� �մϴٸ�... :2",
                                            "�� ���� ��Ź�� �ֽ��ϴ�. :3",
                                            "���� ���͵��� ���� ������� �����մϴ�. :4",
                                            "���� ��ġ����Դϴ� :5",
                                            "���谡�Ը� �����ٸ� �ֺ��� ���ƴٴϴ� Ʈ�ѵ��� ��ġ���ֽñ�ٶ��ϴ�. :6",
                                            "���ŷӰ� �Ͽ� �˼��մϴ�. :7"
                                            });

        talkData.Add(30 + 2000, new string[] { "���̱���! :0",
                                            "���� ����帳�ϴ�! :1",
                                            "������ �ذ����ּ����� �ڼ��� �˷��帮�ڽ��ϴ�. :2",
                                            "�ֱ� �ֹε��� �� �������� ��� �� �� ���� �Ҹ��� ���� �鸰�ٰ� ������ϴ�. :3",
                                            "���� �ִ°� Ʋ�������ϴ�. :4",
                                            "�� ���� �ſ� �����մϴ�. :5",
                                            "�������� ��ġ�Ǿ �������� ������� �ְ�... :6",
                                            "������ ���谡�Ը� �����ٸ� ���� ���ֽǼ� �����ʴϱ�? :7",
                                            "���谡�� �ε� �����Ͻñ� �ٶ��ϴ�. :8"
                                             });

        talkData.Add(40 + 2000, new string[] { "���� �����ϳ�.",
                                            "��� ���п� ����� ���� �������.",
                                            "������ ���ϰ� ������ �ֵ��� ���ְڳ�."
                                             });
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
