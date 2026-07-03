using UnityEngine;

public class AssemblerGameManager : MonoBehaviour
{
    [SerializeField] private Assembler assembler;
    [SerializeField] private Specifications specifications;

    private void OnEnable()
    {
        assembler.OnRobotStatisticsUpdated += specifications.UpdateSpecifications;
    }

    private void OnDisable()
    {
        assembler.OnRobotStatisticsUpdated -= specifications.UpdateSpecifications;
    }

    private void Start()
    {
        RobotStatistics initialStatistics = new RobotStatistics();
        specifications.UpdateSpecifications(initialStatistics);
    }
}
