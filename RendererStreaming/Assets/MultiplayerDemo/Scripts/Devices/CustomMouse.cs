using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomMouse : Mouse,ICustomDevice
{
    public override void MakeCurrent()
    {
    }

    public int UserId { get; set; }
}
