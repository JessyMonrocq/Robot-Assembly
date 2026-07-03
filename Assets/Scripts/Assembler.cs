using System;
using UnityEngine;

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
    public event Action<RobotStatistics> OnRobotStatisticsUpdated;

    [Header("Assembler References")]
    [SerializeField] private ItemSocket headSocket;
    [SerializeField] private ItemSocket bodySocket;
    [SerializeField] private ItemSocket leftArmSocket;
    [SerializeField] private ItemSocket rightArmSocket;
    [SerializeField] private ItemSocket legsSocket;

    private RobotPartData[] robotPartsData = new RobotPartData[5];

    private void Awake()
    {
        InitializeAssembler();
    }

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
    }

    private void OnDisable()
    {
        if (robotPartsData == null)
        {
            return;
        }

        foreach (RobotPartData item in robotPartsData)
        {
            item.Socket.OnItemSocketed -= HandleItemSocketed;
            item.Socket.OnItemRemoved -= HandleItemRemoved;
        }
    }

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
                currentStatistics.Computation += item.RobotPart.Stats.Computation;
                currentStatistics.EnergyConsumption += item.RobotPart.Stats.EnergyConsumption;
                currentStatistics.Weight += item.RobotPart.Stats.Weight;
            }
        }

        OnRobotStatisticsUpdated?.Invoke(currentStatistics);
    }
}
