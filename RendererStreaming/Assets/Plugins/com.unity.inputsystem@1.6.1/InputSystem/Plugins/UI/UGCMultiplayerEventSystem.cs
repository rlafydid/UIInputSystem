using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class UGCMultiplayerEventSystem : EventSystem
{
    public static UGCMultiplayerEventSystem Instance { get; private set; }

    private Dictionary<BaseRaycaster, int> _multiplayerRaycasters = new();
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public void Register(BaseRaycaster raycaster, int userId)
    {
        Debug.Log($"注册用户ID {userId}");
        _multiplayerRaycasters.Add(raycaster, userId);
    }

    public new void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
    {
        if (eventData is ExtendedPointerEventData extendedPointerEventData && extendedPointerEventData.device is ICustomDevice customDevice)
        {
            var modules = RaycasterManager.GetRaycasters();
            var modulesCount = modules.Count;
            for (int i = 0; i < modulesCount; ++i)
            {
                var module = modules[i];
                if (module == null || !module.IsActive())
                    continue;

                if (_multiplayerRaycasters.TryGetValue(module, out var userId) && customDevice.UserId == userId)
                {
                    eventData.displayIndex = 1;
                    module.Raycast(eventData, raycastResults);
                    foreach (var raycast in raycastResults)
                    {
                        Debug.Log($"检测到 {raycast.gameObject} index {raycast.displayIndex} userId {userId} pos {eventData.position}");
                    }
                }
            }
            raycastResults.Sort(RaycastComparer);
        }
        else
        {
            base.RaycastAll(eventData, raycastResults);
        }
    }

   private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
    {
        if (lhs.module != rhs.module)
        {
            var lhsEventCamera = lhs.module.eventCamera;
            var rhsEventCamera = rhs.module.eventCamera;
            if (lhsEventCamera != null && rhsEventCamera != null && lhsEventCamera.depth != rhsEventCamera.depth)
            {
                // need to reverse the standard compareTo
                if (lhsEventCamera.depth < rhsEventCamera.depth)
                    return 1;
                if (lhsEventCamera.depth == rhsEventCamera.depth)
                    return 0;

                return -1;
            }

            if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
                return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);

            if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
                return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
        }

        // Renderer sorting
        if (lhs.sortingLayer != rhs.sortingLayer)
        {
            // Uses the layer value to properly compare the relative order of the layers.
            var rid = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
            var lid = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
            return rid.CompareTo(lid);
        }

        if (lhs.sortingOrder != rhs.sortingOrder)
            return rhs.sortingOrder.CompareTo(lhs.sortingOrder);

        // comparing depth only makes sense if the two raycast results have the same root canvas (case 912396)
        if (lhs.depth != rhs.depth && lhs.module.rootRaycaster == rhs.module.rootRaycaster)
            return rhs.depth.CompareTo(lhs.depth);

        if (lhs.distance != rhs.distance)
            return lhs.distance.CompareTo(rhs.distance);

        #if PACKAGE_PHYSICS2D
        // Sorting group
        if (lhs.sortingGroupID != SortingGroup.invalidSortingGroupID && rhs.sortingGroupID != SortingGroup.invalidSortingGroupID)
        {
            if (lhs.sortingGroupID != rhs.sortingGroupID)
                return lhs.sortingGroupID.CompareTo(rhs.sortingGroupID);
            if (lhs.sortingGroupOrder != rhs.sortingGroupOrder)
                return rhs.sortingGroupOrder.CompareTo(lhs.sortingGroupOrder);
        }
        #endif

        return lhs.index.CompareTo(rhs.index);
    }

}
