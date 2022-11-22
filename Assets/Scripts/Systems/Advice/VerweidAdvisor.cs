using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerweidAdvisor : MonoBehaviour
{
    private Queue<Advice> adviceQueue = new Queue<Advice>();


    private void Start()
    {
        adviceQueue.Enqueue(new WeatherAdvice());
        adviceQueue.Enqueue(new BaseAdvice());
        adviceQueue.Enqueue(new InputAdvice());
        GetAdvice();
    }

    private void GetAdvice()
    {
        int queueLength = adviceQueue.Count;

        for (int i = queueLength; i > 0; i--)
        {
            var advice = adviceQueue.Dequeue();
            advice.GiveAdvice();
        }
    }
}
