using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[Serializable]
public class Player
{
    public MultiplayerEventSystem system;
    public bool touch;
}

public class Test : MonoBehaviour
{
    [SerializeField]
    public List<Player> players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (players == null)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var player in players)
            {
                if(!player.touch)
                    continue;
                Debug.Log($"执行 {player.system.gameObject.name}");
                var oointerEvent = new PointerEventData(player.system);
                oointerEvent.button = PointerEventData.InputButton.Left;
                oointerEvent.position = Input.mousePosition; // 这里就是模拟鼠标位置
            
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                player.system.RaycastAll(oointerEvent, raycastResults);
            
                // 如果找到 Interactive Elements，则触发 Press 和 Click 事件。
                if (raycastResults.Count > 0)
                {
                    Debug.Log($"模拟事件成功 {player.system.gameObject.name}");
                    foreach (var result in raycastResults)
                    {
                        Debug.Log($"检测到的物体 {result.gameObject}");
                    }
                    ExecuteEvents.Execute(player.system.playerRoot, oointerEvent, ExecuteEvents.pointerDownHandler);
                    ExecuteEvents.Execute(player.system.playerRoot, oointerEvent, ExecuteEvents.pointerClickHandler);
                    ExecuteEvents.Execute(player.system.playerRoot, oointerEvent, ExecuteEvents.pointerUpHandler);
                }
            }
        }
    }
}
