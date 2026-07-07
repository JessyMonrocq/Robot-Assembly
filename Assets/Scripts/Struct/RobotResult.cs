using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RobotStatResult
{
    public float Required;
    public float Result;
    public SatisfactionLevel.SatisfactionDegree SatisfactionDegree;
}

[Serializable]
public class RobotResult
{
    public RobotStatResult Armor;
    public RobotStatResult Mobility;
    public RobotStatResult Strength;
    public RobotStatResult Computing;
    public RobotStatResult Energy;
    public RobotStatResult Weight;

    public IEnumerable<(string Name, RobotStatResult Stat)> EnumerateStats()
    {
        yield return ("Armor", Armor);
        yield return ("Mobility", Mobility);
        yield return ("Strength", Strength);
        yield return ("Computing", Computing);
        yield return ("Energy", Energy);
        yield return ("Weight", Weight);
    }
}
