using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour,IPointerClickHandler,IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"click {this.gameObject.name}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"OnPointerDown {this.gameObject.name}");
    }
}
