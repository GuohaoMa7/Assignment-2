using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO; 
using System;

public class InputHandler : MonoBehaviour
{

    private DateTime startTime; 
    private DateTime endTime;
    private bool timingStarted = false; 

    private Camera _mainCamera;
    private ColorChanger _colorChanger;

    // ���Ŀ���б�͵�ǰĿ������
    public List<GameObject> targets;
    private int currentTargetIndex = 0;

    private void Awake()
    {

        _mainCamera = Camera.main;
        _colorChanger = GetComponent<ColorChanger>();

        // ��ʼ��Ŀ���б���ɫ�����õ�һ��Ŀ��Ϊ��ɫ
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

        if (!rayHit.collider) return;

        Debug.Log(rayHit.collider.gameObject.name);



        //start timeing
         if (rayHit.collider.gameObject.CompareTag("ball") && !timingStarted)
        {
            startTime = DateTime.Now;
            timingStarted = true;
        }







        // ȷ�ϵ�����ǵ�ǰĿ��
        if (rayHit.collider.gameObject == targets[currentTargetIndex])
        {
            // ����ǰĿ����ɫ�Ļذ�ɫ
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.white);

            // ѡ����һ��Ŀ�꣨�Խ��߷���
            currentTargetIndex = (currentTargetIndex + (targets.Count / 2)) % targets.Count;

            // ������Ŀ��Ϊ��ɫ
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.red);

            //finish timeing
            if (currentTargetIndex == 0 && timingStarted)
            {
                endTime = DateTime.Now;
                TimeSpan totalTime = endTime - startTime;
                Debug.Log("Total time: " + totalTime);

                string filePath = Application.dataPath + "/ClickTime.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Total time: " + totalTime);
                }

                timingStarted = false;
            }
        }
    }
}
