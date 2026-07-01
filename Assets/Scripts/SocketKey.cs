using System;

[Serializable]
public struct SocketKey
{
    public enum SocketType
    {
        HeadPart,
        BodyPart,
        LeftArmPart,
        RightArmPart,
        LegsPart
    }

    public SocketType socketType;
}
