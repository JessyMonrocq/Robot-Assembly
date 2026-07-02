using UnityEngine;

[CreateAssetMenu(fileName = "RobotPartSO", menuName = "Scriptable Objects/RobotPartSO")]
public class RobotPartSO : ScriptableObject
{
    public enum RobotPartType
    {
        Head,
        Body,
        LeftArm,
        RightArm,
        Legs
    }

    public RobotPartType PartType;

    public string PartName;
    [TextArea] public string PartDescription;
    public Sprite PartImage;
}
