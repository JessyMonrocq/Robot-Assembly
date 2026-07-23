using UnityEngine;

[CreateAssetMenu(fileName = "SatisfactionLevelsSO", menuName = "Scriptable Objects/SatisfactionLevelsSO")]
public class SatisfactionLevelsSO : ScriptableObject
{
    #region Inspector Fields
    [Header("Satisfaction levels ")]
    public SatisfactionLevel unsatisfied;
    public SatisfactionLevel poor;
    public SatisfactionLevel average;
    public SatisfactionLevel good;
    public SatisfactionLevel perfect;

    [Header("Satisfaction levels threshold")]
    [Range(0f, 1f)] public float poorThreshold = (1f/3f);

    [Range(0f, 1f)] public float averageThreshold = (2f/3f);

    [Range(0f, 1f)] public float goodThreshold = 1f;

    [Header("UI partitioning")]
    [Min(1)] public int partitions = 3;

    public float Partition => 1f / Mathf.Max(1, partitions);
    #endregion

    #region Methods
    public SatisfactionLevel GetByDegree(SatisfactionLevel.SatisfactionDegree degree)
    {
        return degree switch
        {
            SatisfactionLevel.SatisfactionDegree.Unsatisfied => unsatisfied,
            SatisfactionLevel.SatisfactionDegree.Poor => poor,
            SatisfactionLevel.SatisfactionDegree.Average => average,
            SatisfactionLevel.SatisfactionDegree.Good => good,
            SatisfactionLevel.SatisfactionDegree.Perfect => perfect,
            _ => unsatisfied
        };
    }

    public bool TryGetByName(string saved, out SatisfactionLevel sl)
    {
        sl = unsatisfied;
        if (string.IsNullOrWhiteSpace(saved)) return false;
        switch (saved.Trim().ToLowerInvariant())
        {
            case "unsatisfied": sl = unsatisfied; return true;
            case "poor": sl = poor; return true;
            case "average": sl = average; return true;
            case "good": sl = good; return true;
            case "perfect": sl = perfect; return true;
            default: return false;
        }
    }

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
    #endregion
}
