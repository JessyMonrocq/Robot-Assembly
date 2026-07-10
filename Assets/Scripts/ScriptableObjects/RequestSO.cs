using UnityEngine;

[CreateAssetMenu(fileName = "RequestSO", menuName = "Scriptable Objects/RequestSO")]
public class RequestSO : ScriptableObject
{
    public RobotStatistics RequestStats;
    public ChronoFormat Chrono;
}
