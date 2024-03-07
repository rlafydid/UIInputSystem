using System.Collections;
using System.Collections.Generic;
using Unity.RenderStreaming.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class MultiplayerInputSimulator : MonoBehaviour
{
    public RemoteInput RemoteInput
    {
        get => _remoteInput;
    }

    private RemoteInput _remoteInput;

    protected InputUser InputUser { get; set; }

    protected virtual void Start()
    {
        if (!Application.isPlaying)
            return;

        InputUser user = InputUser.CreateUserWithoutPairedDevices();
        user = InputUser.PerformPairingWithDevice(InputSystem.AddDevice<CustomMouse>(), user);
        user = InputUser.PerformPairingWithDevice(InputSystem.AddDevice<CustomKeyboard>(), user);
        user = InputUser.PerformPairingWithDevice(InputSystem.AddDevice<CustomGamepad>(), user);
        user = InputUser.PerformPairingWithDevice(InputSystem.AddDevice<CustomTouchscreen>(), user);
        _remoteInput = new RemoteInput(ref user);

        InputUser = user;
        this.GetComponent<MultiplayerGraphicRaycaster>()?.SetInputUser(user);
    }
}
