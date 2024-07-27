using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class InputHandler : MonoBehaviour
{
    private DateTime? previousClickTime = null; 
    private Camera _mainCamera;
    private ColorChanger _colorChanger;

    public List<GameObject> targets;
    private int currentTargetIndex = 0;

    //change this when change a new scene
    private string technique = "Mouse";
    private double width = 0.5;
    private double amplitude = 5;

    private bool isCorrect = false; 

    private void Awake()
    {
        _mainCamera = Camera.main;
        _colorChanger = GetComponent<ColorChanger>();

        for (int i = 0; i < targets.Count; i++)
        {
            _colorChanger.ChangeColor(targets[i], Color.white);
        }
        if (targets.Count > 0)
        {
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.red);
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(
            _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue())
        );

        DateTime currentClickTime = DateTime.Now;

        if (!rayHit.collider)
        {
            LogClickData(currentClickTime, false);
            return;
        }

        Debug.Log(rayHit.collider.gameObject.name);

        // Check if clicking outside the ball
        if (!rayHit.collider.CompareTag("ball"))
        {

            isCorrect = false;
            LogClickData(currentClickTime, isCorrect);
            return;
        }

        // Check if clicking the ball
        if (rayHit.collider.gameObject == targets[currentTargetIndex])
        {
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.white);


            currentTargetIndex = (currentTargetIndex + (targets.Count / 2)) % targets.Count;
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.red);


            isCorrect = true;
            LogClickData(currentClickTime, isCorrect);
        }
        else
        {

            isCorrect = false;
            LogClickData(currentClickTime, isCorrect);
        }
    }

    private void LogClickData(DateTime currentClickTime, bool isCorrect)
    {
        if (previousClickTime.HasValue)
        {
            TimeSpan timeInterval = currentClickTime - previousClickTime.Value;
            double timeSeconds = timeInterval.TotalSeconds;
            LogData(technique, width, amplitude, timeSeconds, isCorrect);
        }
        else
        {
            LogData(technique, width, amplitude, 0, isCorrect); 
        }

        previousClickTime = currentClickTime;
    }

    private void LogData(string technique, double width, double amplitude, double time, bool isCorrect)
    {
        string filePath = Application.dataPath + "/ClickTime.txt";
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{technique}, {width}, {amplitude}, {time}, {(isCorrect ? 1 : 0)}");
        }
    }
}
