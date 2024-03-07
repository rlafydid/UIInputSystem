using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.RenderStreaming.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum SimulateType
{
    Click,
    Drag
}

public enum DragDirection
{
    Vertical,
    Horizontal
}

public class MultiplayerInputSimulatorTest : MonoBehaviour
{

    public float dragStartX = 100;
    public float dragStartY = 100;

    public bool isOpen = false;
    public DragDirection _direction = DragDirection.Vertical;

    public float dragSpeed = 10;
    
    private SimulateType _simulateType = SimulateType.Click;

    private float _timer;
    private float dragValue = 100;

    private bool isPressed = false;

    private MultiplayerInputSimulator _inputSimulator;
    private RemoteInput RemoteInput
    {
        get => _inputSimulator.RemoteInput;
    }

    public async void Start()
    {
        dragValue = _direction == DragDirection.Horizontal ? dragStartX : dragStartY;
        _inputSimulator = this.GetComponent<MultiplayerInputSimulator>();
        
        Debug.Log($"target displau {this.GetComponent<GraphicRaycaster>().eventCamera.targetDisplay}");
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.A].isPressed && !isPressed)
        {
            SimulateClick(Mouse.current.position.ReadValue());
        }
        if (!isOpen)
            return;
        _timer += Time.deltaTime;
        switch (_simulateType)
        {
            case SimulateType.Click:
                if (_timer > 4)
                {
                    // SimulateClick();
                    _timer = 0;
                    _simulateType = SimulateType.Drag;
                }
                break;
            case SimulateType.Drag:
                if (_timer < 5)
                {
                    if (_timer > 1)
                    {
                        dragValue += Time.deltaTime * dragSpeed;
                        switch (_direction)
                        {
                            case DragDirection.Horizontal:
                                RemoteInput.ProcessMouseMoveEvent((short)dragValue, (short)dragStartY, 1);
                                break;
                            case DragDirection.Vertical:
                                RemoteInput.ProcessMouseMoveEvent((short)dragStartX, (short)dragValue, 1);
                                break;
                        }
                    }
                }
                else
                {
                    _simulateType = SimulateType.Click;
                    _timer = 0;
                    switch (_direction)
                    {
                        case DragDirection.Horizontal:
                            RemoteInput.ProcessMouseMoveEvent((short)dragValue, (short)dragStartY, 0);
                            break;
                        case DragDirection.Vertical:
                            RemoteInput.ProcessMouseMoveEvent((short)dragStartX, (short)dragValue, 0);
                            break;
                    }
                    dragValue = _direction == DragDirection.Horizontal ? dragStartX : dragStartY;

                }
                break;
        }

        
    }



    async void SimulateClick(Vector2 pos)
    {
        isPressed = true;
        Debug.Log($"$frameCount {Time.frameCount} 开始模拟点击按钮");
        RemoteInput.ProcessMouseMoveEvent((short)pos.x, (short)pos.y, 1);
        await Task.Delay(100);
        RemoteInput.ProcessMouseMoveEvent((short)pos.x, (short)pos.y, 0);
        isPressed = false;
    }

}
