using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct RobotPartData
{
    public RobotPartSO.RobotPartType PartType;
    public RobotPartSO RobotPart;
    public ItemSocket Socket;

    public RobotPartData(RobotPartSO.RobotPartType partType, RobotPartSO robotPart, ItemSocket socket)
    {
        PartType = partType;
        RobotPart = robotPart;
        Socket = socket;
    }
}

public class Assembler : MonoBehaviour
{
    #region Inspector Fields
    public event Action<RobotStatistics> OnRobotStatisticsUpdated;

    [Header("Assembler References")]
    [SerializeField] private ItemSocket headSocket;
    [SerializeField] private ItemSocket bodySocket;
    [SerializeField] private ItemSocket leftArmSocket;
    [SerializeField] private ItemSocket rightArmSocket;
    [SerializeField] private ItemSocket legsSocket;

    [Header("Removal Settings")]
    [SerializeField] private float shakeDuration = 5f;
    [SerializeField] private float shakeIntensity = 0.2f;
    [SerializeField] private int shakeVibrato = 3;

    [Header("Confirmation References")]
    [SerializeField] private Button confirmAssemblyButton;

    public enum AssemblerState
    {
        Empty,
        NotEmpty,
        Full
    }

    private AssemblerState assemblerState;
    private RobotPartData[] robotPartsData = new RobotPartData[5];
    #endregion

    #region Unity Methods
    private void Awake()
    {
        InitializeAssembler();
    }

    private void OnDisable()
    {
        if (robotPartsData == null)
        {
            return;
        }

        foreach (RobotPartData item in robotPartsData)
        {
            if (item.Socket != null)
            {
                continue;
            }

            item.Socket.OnItemSocketed -= HandleItemSocketed;
            item.Socket.OnItemRemoved -= HandleItemRemoved;
        }
    }
    #endregion

    #region Public Methods
    public void InitializeAssembler()
    {
        robotPartsData[0] = new RobotPartData(RobotPartSO.RobotPartType.Head, null, headSocket);
        robotPartsData[1] = new RobotPartData(RobotPartSO.RobotPartType.Body, null, bodySocket);
        robotPartsData[2] = new RobotPartData(RobotPartSO.RobotPartType.LeftArm, null, leftArmSocket);
        robotPartsData[3] = new RobotPartData(RobotPartSO.RobotPartType.RightArm, null, rightArmSocket);
        robotPartsData[4] = new RobotPartData(RobotPartSO.RobotPartType.Legs, null, legsSocket);

        foreach (RobotPartData item in robotPartsData)
        {
            item.Socket.InitializeSocket();

            item.Socket.OnItemSocketed += HandleItemSocketed;
            item.Socket.OnItemRemoved += HandleItemRemoved;
        }

        confirmAssemblyButton.interactable = false;
        assemblerState = AssemblerState.Empty;
    }

    public void ResetAssembler()
    {
        for (int i = 0; i < robotPartsData.Length; i++)
        {
            robotPartsData[i].RobotPart = null;
            robotPartsData[i].Socket.RemoveItem(destroy: true);
        }
        CalculateCurrentStatistics();
    }
    public void PrepareItemRemoval(bool remove)
    {
        if (remove)
        {
            if (assemblerState != AssemblerState.Empty)
            {
                StartCoroutine(RemoveAllPartsCoroutine());
            }
        }
        else
        {
            StopAllCoroutines();

            headSocket.transform.DOKill();
            bodySocket.transform.DOKill();
            leftArmSocket.transform.DOKill();
            rightArmSocket.transform.DOKill();
            legsSocket.transform.DOKill();

            headSocket.transform.DOScale(Vector3.one, 0.2f);
            bodySocket.transform.DOScale(Vector3.one, 0.2f);
            leftArmSocket.transform.DOScale(Vector3.one, 0.2f);
            rightArmSocket.transform.DOScale(Vector3.one, 0.2f);
            legsSocket.transform.DOScale(Vector3.one, 0.2f);
        }

    }
    #endregion

    #region Private Methods
    private void HandleItemSocketed(RobotPartSO robotPart)
    {
        for (int i = 0; i < robotPartsData.Length; i++)
        {
            if (robotPartsData[i].PartType == robotPart.PartType)
            {
                robotPartsData[i].RobotPart = robotPart;
                break;
            }
        }

        CalculateCurrentStatistics();
    }

    private void HandleItemRemoved(RobotPartSO robotPart)
    {
        for (int i = 0; i < robotPartsData.Length; i++)
        {
            if (robotPartsData[i].PartType == robotPart.PartType)
            {
                robotPartsData[i].RobotPart = null;
                break;
            }
        }

        CalculateCurrentStatistics();
    }

    private void CalculateCurrentStatistics()
    {
        RobotStatistics currentStatistics = new RobotStatistics();
        foreach (RobotPartData item in robotPartsData)
        {
            if (item.RobotPart != null)
            {
                currentStatistics.Armor += item.RobotPart.Stats.Armor;
                currentStatistics.Mobility += item.RobotPart.Stats.Mobility;
                currentStatistics.Strength += item.RobotPart.Stats.Strength;
                currentStatistics.Computing += item.RobotPart.Stats.Computing;
                currentStatistics.Energy += item.RobotPart.Stats.Energy;
                currentStatistics.Weight += item.RobotPart.Stats.Weight;
            }
        }

        CheckAssemblerState();
        OnRobotStatisticsUpdated?.Invoke(currentStatistics);
    }

    private void CheckAssemblerState()
    {
        bool empty = true;
        bool full = true;
        foreach (RobotPartData item in robotPartsData)
        {
            if (item.RobotPart != null)
            {
                empty = false;
            }
            else
            {
                full = false;
            }
        }

        if (empty)
        {
            assemblerState = AssemblerState.Empty;
        }
        else
        {
            assemblerState = full ? AssemblerState.Full : AssemblerState.NotEmpty;
        }

        confirmAssemblyButton.interactable = full;
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator RemoveAllPartsCoroutine()
    {
        headSocket.transform.DOShakeScale(shakeDuration, shakeIntensity, shakeVibrato);
        bodySocket.transform.DOShakeScale(shakeDuration, shakeIntensity, shakeVibrato);
        leftArmSocket.transform.DOShakeScale(shakeDuration, shakeIntensity, shakeVibrato);
        rightArmSocket.transform.DOShakeScale(shakeDuration, shakeIntensity, shakeVibrato);
        legsSocket.transform.DOShakeScale(shakeDuration, shakeIntensity, shakeVibrato);

        yield return new WaitForSeconds(shakeDuration);

        foreach (RobotPartData item in robotPartsData)
        {
            if (item.RobotPart != null)
            {
                item.Socket.RemoveItem(destroy: true);
            }
        }

        CheckAssemblerState();
    }
    #endregion
}
