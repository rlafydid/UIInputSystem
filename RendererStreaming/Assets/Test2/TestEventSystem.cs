using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class TestEventSystem : MonoBehaviour
{
    public GameObject uiA, uiB;

    private Camera camA, camB;
    private MultiplayerEventSystem eventSystemA, eventSystemB;

    void Start()
    {
        // 获取 UI A 和 B 对应的 Camera 和 EventSystem
        camA = uiA.GetComponent<Canvas>().worldCamera;
        camB = uiB.GetComponent<Canvas>().worldCamera;
        eventSystemA = uiA.GetComponent<MultiplayerEventSystem>();
        eventSystemB = uiB.GetComponent<MultiplayerEventSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector2 mousePosition = Input.mousePosition;

            // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI A，并将鼠标位置传递给其他客户端。
            RpcSimulateMouseClickOnUI(mousePosition, "A");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector2 mousePosition = Input.mousePosition;

            // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI B，并将鼠标位置传递给其他客户端。
            RpcSimulateMouseClickOnUI(mousePosition, "B");
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 mousePosition = Input.mousePosition;

            // 在本地玩家上创建一个 RPC 请求，该请求将参数设置为 UI B，并将鼠标位置传递给其他客户端。
            RpcSimulateMouseClickOnUI(mousePosition, "A");
            RpcSimulateMouseClickOnUI(mousePosition, "B");
        }
    }

    void RpcSimulateMouseClickOnUI(Vector2 mousePosition, string uiName)
    {
        // 根据所需的 UI 名称选择正确的 Camera 和 EventSystem
        Camera cam = null;
        EventSystem eventSystem = null;
        if (uiName.Equals("A"))
        {
            cam = camA;
            eventSystem = eventSystemA;
        }
        else if (uiName.Equals("B"))
        {
            cam = camB;
            eventSystem = eventSystemB;
        }
        else
        {
            Debug.LogErrorFormat("Unknown UI name: {0}", uiName);
            return;
        }

        // 创建一个新的 PointerEventData 实例，并根据鼠标位置设置其位置。
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.button = PointerEventData.InputButton.Left;
        eventData.position = mousePosition;

        // 使用正确的 Camera 和 EventSystem 进行 RaycastAll 操作，以查找与鼠标位置重叠的控件。
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        Debug.Log($"处理 {uiName}");

        foreach (var result in results)
        {
            Debug.Log($"检测到 {result.gameObject}事件");
            if (result.gameObject.layer == eventSystem.gameObject.layer)
            {
                Debug.Log($"执行 {uiName}事件");
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerUpHandler);
            }
        }
    }
}
