using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    private ColorChanger _colorChanger;

    // 添加目标列表和当前目标索引
    public List<GameObject> targets;
    private int currentTargetIndex = 0;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _colorChanger = GetComponent<ColorChanger>();

        // 初始化目标列表颜色，设置第一个目标为红色
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

        // 确认点击的是当前目标
        if (rayHit.collider.gameObject == targets[currentTargetIndex])
        {
            // 将当前目标颜色改回白色
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.white);

            // 选择下一个目标（对角线方向）
            currentTargetIndex = (currentTargetIndex + (targets.Count / 2)) % targets.Count;

            // 设置新目标为红色
            _colorChanger.ChangeColor(targets[currentTargetIndex], Color.red);
        }
    }
}
