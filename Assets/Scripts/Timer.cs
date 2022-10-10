using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    float time;
    private int currentTime = 0;

    public static bool IsOver = false;

    public UnityEvent OnTimeOver;

    private void Start()
    {
      
        StartTimer();
    }

    private void Update()
    {
        // Ÿ�̸Ӱ� ������ �ʾҴٸ�
        if (Timer.IsOver == false && time > 0)
        {
            time -= Time.deltaTime;
            currentTime = (int)time;
            if (currentTime <= 0)
            {
                currentTime = 0;
                StopTimer();
                TimeOver();
            }
        }
    }

    // Ÿ�̸Ӹ� ���ߴ� �޼ҵ�
    public void StopTimer()
    {
        Timer.IsOver = true;
    }

    // Ÿ�̸Ӹ� �����ϴ� �޼ҵ�
    public void StartTimer()
    {
        Timer.IsOver = false;
    }

    public void SetTimer(int sec)
    {
        time = sec;
    }

    // Ÿ�̸Ӱ� �� ���� �� �ߵ��ϴ� �޼ҵ�
    private void TimeOver()
    {
        OnTimeOver.Invoke();
    }
}
