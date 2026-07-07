using UnityEngine;

[CreateAssetMenu(fileName = "SatisfactionLevelsSO", menuName = "Scriptable Objects/SatisfactionLevelsSO")]
public class SatisfactionLevelsSO : ScriptableObject
{
    [Header("Satisfaction levels")]
    [Range(0f, 1f)] public float poorThreshold = (1f/3f);

    [Range(0f, 1f)] public float averageThreshold = (2f/3f);

    [Range(0f, 1f)] public float goodThreshold = 1f;

    [Header("UI partitioning")]
    [Min(1)] public int partitions = 3;

    public float Partition => 1f / Mathf.Max(1, partitions);

    public SatisfactionLevel.SatisfactionDegree MapRatioToSatisfactionDegree(float ratio)
    {
        if (ratio <= 0f) return SatisfactionLevel.SatisfactionDegree.Unsatisfied;
        if (ratio < poorThreshold) return SatisfactionLevel.SatisfactionDegree.Poor;
        if (ratio < averageThreshold) return SatisfactionLevel.SatisfactionDegree.Average;
        if (ratio < goodThreshold) return SatisfactionLevel.SatisfactionDegree.Good;
        return SatisfactionLevel.SatisfactionDegree.Perfect;
    }

    private void OnValidate()
    {
        poorThreshold = Mathf.Clamp01(poorThreshold);
        averageThreshold = Mathf.Clamp01(averageThreshold);
        goodThreshold = Mathf.Clamp01(goodThreshold);

        if (poorThreshold > averageThreshold) averageThreshold = poorThreshold;
        if (averageThreshold > goodThreshold) goodThreshold = averageThreshold;

        partitions = Mathf.Max(1, partitions);
    }
}
