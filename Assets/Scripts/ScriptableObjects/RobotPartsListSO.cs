using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotPartsListSO", menuName = "Scriptable Objects/RobotPartsListSO")]
public class RobotPartsListSO : ScriptableObject
{
    public List<RobotPartSO> RobotPartsList; 
}
