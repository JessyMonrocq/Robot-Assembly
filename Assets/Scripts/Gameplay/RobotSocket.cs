using System;
using UnityEngine;

public class RobotSocket : MonoBehaviour
{
    [Header("Robot Socket Settings")]
    [SerializeField] private Socket socket;
    [SerializeField] private bool useSocketKey = true;
    [SerializeField] private RobotPartSO.RobotPartType socketType;

    public event Action<RobotPartSO> OnItemSocketed;
    public event Action<RobotPartSO> OnItemRemoved;

    public Socket Socket
    {
        get { return socket; }
    }

    private void Reset()
    {
        if (socket == null)
        {
            socket = GetComponent<Socket>();
        }
    }

    private void OnEnable()
    {
        if (socket == null)
        {
            return;
        }

        socket.CompatibilityCheck = IsCompatible;

        socket.OnSocketed += HandleSocketed;
        socket.OnRemoved += HandleRemoved;
    }

    private void OnDisable()
    {
        if (socket == null)
        {
            return;
        }

        socket.OnSocketed -= HandleSocketed;
        socket.OnRemoved -= HandleRemoved;

        socket.CompatibilityCheck = _ => true;
    }

    private bool IsCompatible(GameObject go)
    {
        if (!useSocketKey)
        {
            return true;
        }

        var robotDraggable = go.GetComponent<RobotDraggable>();
        if (robotDraggable != null && robotDraggable.RobotPartSO != null)
        {
            return robotDraggable.RobotPartSO.PartType == socketType;
        }

        /*var dragItem = go.GetComponent<DragItem>();
        if (dragItem != null && dragItem.RobotPartSO != null)
        {
            return dragItem.RobotPartSO.PartType == socketType;
        }*/

        return false;
    }

    private void HandleSocketed(GameObject go)
    {
        RobotPartSO part = null;

        var robotDraggable = go.GetComponent<RobotDraggable>();
        if (robotDraggable != null)
        {
            part = robotDraggable.RobotPartSO;
        }
        else
        {
            /*var draggableItem = go.GetComponent<DraggableItem>();
            if (draggableItem != null)
            {
                part = draggableItem.RobotPartSO;
            }*/
        }

        if (part != null)
        {
            OnItemSocketed?.Invoke(part);
        }
    }

    private void HandleRemoved(GameObject go)
    {
        RobotPartSO part = null;

        var robotDraggable = go.GetComponent<RobotDraggable>();
        if (robotDraggable != null)
        {
            part = robotDraggable.RobotPartSO;
        }
        else
        {
            /*var draggableItem = go.GetComponent<DraggableItem>();
            if (draggableItem != null)
            {
                part = draggableItem.RobotPartSO;
            }*/
        }

        if (part != null)
        {
            OnItemRemoved?.Invoke(part);
        }
    }
}