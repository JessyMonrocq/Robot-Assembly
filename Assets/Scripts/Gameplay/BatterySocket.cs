using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BatterySocket : MonoBehaviour
{
    [Header("Battery Socket Settings")]
    [SerializeField] private Socket socket;
    [SerializeField] private bool useSocketKey = true;
    [SerializeField] private bool lockOnSocketed = false;

    public event Action<BatteryDraggable> OnBatterySocketed;
    public event Action<BatteryDraggable> OnBatteryRemoved;

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

        var battery = go.GetComponent<BatteryDraggable>();
        return battery != null;
    }

    private void HandleSocketed(GameObject go)
    {
        StartCoroutine(HandleSocketedCoroutine(go));
    }

    private IEnumerator HandleSocketedCoroutine(GameObject go)
    {
        if (go == null)
        {
            yield break;
        }

        var battery = go.GetComponent<BatteryDraggable>();
        var drag = go.GetComponent<DragItem>();
        var rt = go.GetComponent<RectTransform>();

        if (transform != null && go.transform.parent != socket.transform)
        {
            go.transform.SetParent(socket.transform);
        }

        if (rt != null)
        {
            rt.DOKill();
            yield return rt.DOAnchorPos(Vector2.zero, 0.1f).SetEase(Ease.OutQuad).WaitForCompletion();
        }

        if (battery != null)
        {
            OnBatterySocketed?.Invoke(battery);
        }

        if (lockOnSocketed)
        {
            if (drag != null)
            {
                drag.SetDraggable(false);
            }

            if (socket != null)
            {
                socket.CanSocket = false;
                socket.CompatibilityCheck = _ => false;

                socket.OnSocketed -= HandleSocketed;
                socket.OnRemoved -= HandleRemoved;
            }
        }
    }

    private void HandleRemoved(GameObject go)
    {
        var battery = go.GetComponent<BatteryDraggable>();
        if (battery != null)
        {
            OnBatteryRemoved?.Invoke(battery);
        }
    }
}