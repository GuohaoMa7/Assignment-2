using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class timeing : MonoBehaviour
{
    private bool timing = false;
    private float startTime;
    private float endTime;
    public List<float> clickTimes = new List<float>();
    private int targetCount = 9;

    private void Start()
    {

    }

    public void StartTiming()
    {
        timing = true;
        startTime = Time.time;
    }

    public void EndTiming()
    {
        timing = false;
        endTime = Time.time;
        float totalTime = endTime - startTime;
        Debug.Log("Total Time: " + totalTime);


        ExportTime(totalTime);
    }

    public void RecordClickTime()
    {
        if (timing)
        {
            float currentTime = Time.time;
            clickTimes.Add(currentTime);
            Debug.Log("Click " + clickTimes.Count + " at " + currentTime);
            if (clickTimes.Count >= targetCount)
            {
                EndTiming();
            }
        }
    }

    private void ExportTime(float totalTime)
    {
        string filePath = Application.dataPath + "/ClickTimes.csv";


        StreamWriter writer;
        if (!File.Exists(filePath))
        {
            writer = new StreamWriter(filePath);
            writer.WriteLine("Total Time");
        }
        else
        {
            writer = File.AppendText(filePath);
        }

        writer.WriteLine(totalTime.ToString());
        writer.Close();
    }
}
