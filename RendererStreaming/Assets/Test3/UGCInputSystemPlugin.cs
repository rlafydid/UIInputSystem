using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class UGCInputSystemPlugin : MonoBehaviour
{
    internal Touchscreen SimulatorTouchscreen;

    private bool m_InputSystemEnabled;
    private bool m_Quitting;
    private List<InputDevice> m_DisabledDevices;

    public string title => "Input System";

    public int touchId;

    public Camera uiCamera;
    
    public async void Start()
    {
        // string deviceName = "My Custom Device";
        // var layout = new InputDeviceDescription();
        // layout.deviceClass = InputDeviceClass.GameController; // 设备类型
        // layout.interfaceName = typeof(IMyInterface).FullName; // 外部调用接口
        // layout.product = deviceName;
            SimulatorTouchscreen = InputSystem.AddDevice<Touchscreen>($"Device Simulator Touchscreen {touchId}");
            // uiCamera.targetDisplay = touchId;

    }
    // private void OnMouseDown(MouseDownEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.Start);
    //
    // private void OnMouseMove(MouseMoveEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.Move);
    //
    // private void OnMouseUp(MouseUpEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.End);
    //
    // private void OnMouseLeave(MouseLeaveEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.End)
    // ;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (touchId != 0)
                return;
            Vector2 mousePosition = Input.mousePosition;
            Debug.Log(mousePosition);
            // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI A，并将鼠标位置传递给其他客户端。
            Click(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (touchId != 1)
                return;
            
            Vector2 mousePosition = Input.mousePosition;

            // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI B，并将鼠标位置传递给其他客户端。
            Click(mousePosition);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 mousePosition = Input.mousePosition;
            Click(mousePosition);
        }
    }

    internal async void Click(Vector3 position)
    {
        // Input System does not accept 0 as id

        InputSystem.QueueStateEvent(SimulatorTouchscreen,
            new TouchState
            {
                touchId = 0,
                phase = TouchPhase.Began,
                position = position
            });

        InputSystem.QueueStateEvent(SimulatorTouchscreen,
            new TouchState
            {
                touchId = 0,
                phase = TouchPhase.Ended,
                position = position
            });
    }
}
