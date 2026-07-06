using System;
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
}
