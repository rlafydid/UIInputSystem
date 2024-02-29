using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.RenderStreaming.Samples;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class UGCInputSystemPlugin : MonoBehaviour
{
    internal Mouse SimulatorTouchscreen;

    private bool m_InputSystemEnabled;
    private bool m_Quitting;
    private List<InputDevice> m_DisabledDevices;

    public string title => "Input System";

    public int touchId;

    public Camera uiCamera;

    private RemoteInput _remoteInput;
    
    public async void Start()
    {
        // string deviceName = "My Custom Device";
        // var layout = new InputDeviceDescription();
        // layout.deviceClass = InputDeviceClass.GameController; // 设备类型
        // layout.interfaceName = typeof(IMyInterface).FullName; // 外部调用接口
        // layout.product = deviceName;
        // SimulatorTouchscreen = InputSystem.AddDevice<Mouse>($"Device Simulator Touchscreen {touchId}");
        // InputSystem.EnableDevice(SimulatorTouchscreen);

        // uiCamera.targetDisplay = touchId;
        _remoteInput = RemoteInputReceiver.Create();
    }
    // private void OnMouseDown(MouseDownEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.Start);
    //
    // private void OnMouseMove(MouseMoveEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.Move);
    //
    // private void OnMouseUp(MouseUpEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.End);
    //
    // private void OnMouseLeave(MouseLeaveEvent evt) => this.SendMouseEvent((IMouseEvent) evt, MousePhase.End)
    // ;

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         if (touchId != 0)
    //             return;
    //         Vector2 mousePosition = Input.mousePosition;
    //         Debug.Log(mousePosition);
    //         // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI A，并将鼠标位置传递给其他客户端。
    //         Click1(mousePosition);
    //         
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.B))
    //     {
    //         if (touchId != 1)
    //             return;
    //         
    //         Vector2 mousePosition = Input.mousePosition;
    //
    //         // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI B，并将鼠标位置传递给其他客户端。
    //         // Click(mousePosition);
    //         Click1(mousePosition);
    //     }
    //     
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         Vector2 mousePosition = Input.mousePosition;
    //         // Click(mousePosition);
    //         Click1(mousePosition);
    //     }
    // }
    private UnityEngine.Touch m_NextTouch;

    internal async void Click(Vector3 position)
    {
        // Input System does not accept 0 as id

        InputSystem.QueueStateEvent(SimulatorTouchscreen,
            new MouseState()
            {
                buttons = (int)MouseButton.Left,
                position = position
            });
        
        InputSystem.QueueStateEvent(SimulatorTouchscreen,
            new MouseState()
            {
                buttons = 0,
                position = position
            });
        // Input.SimulateTouch(this.m_NextTouch);
    }

    async void Click1(Vector3 mousePosition)
    {
        _remoteInput.ProcessMouseMoveEvent((short)mousePosition.x, (short)mousePosition.y, 0);
        await Task.Delay(200);

        _remoteInput.ProcessMouseMoveEvent((short)mousePosition.x, (short)mousePosition.y, 1);

        await Task.Delay(200);
        
        _remoteInput.ProcessMouseMoveEvent((short)mousePosition.x, (short)mousePosition.y, 0);

    }
    
    private float _timer;
    private float y = 200;
    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 2)
        {
            SimulateClick();
            _timer = 0;
        }

        // y += Time.deltaTime * 10;
        // _remoteInput.ProcessMouseMoveEvent(200, (short)y, 1);
    }

    async void SimulateClick()
    {
        Debug.Log($"开始模拟点击按钮");
        _remoteInput.ProcessMouseMoveEvent(200,100, 1);
        await Task.Delay(100);
        _remoteInput.ProcessMouseMoveEvent(200,100, 0);
    }

}
