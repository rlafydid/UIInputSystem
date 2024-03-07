using System.Collections.Generic;
using System.Linq;
using Unity.RenderStreaming.Samples;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;

namespace UnityEngine.UI
{
    public class MultiplayerGraphicRaycaster : GraphicRaycaster
    {
        private InputUser _inputUser;
        private HashSet<int> _pairedDevices;

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (_pairedDevices == null ||
                (eventData is ExtendedPointerEventData data && _pairedDevices.Contains(data.device.deviceId)))
            {
                eventData.position -= RemoteInput.Offset;
                base.Raycast(eventData, resultAppendList);
            }
        }

        public void SetInputUser(InputUser user)
        {
            _inputUser = user;
            _pairedDevices = new HashSet<int>(_inputUser.pairedDevices.Select(d => d.deviceId));
        }
    }
}
