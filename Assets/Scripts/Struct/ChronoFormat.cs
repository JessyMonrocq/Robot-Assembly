using System;
using UnityEngine;

[Serializable]
public struct ChronoFormat
{
    [Min(0)] public int minutes;
    [Range(0, 59)] public int seconds;

    public float GetTimeInSeconds()
    {
        return (minutes * 60f) + seconds;
    }

    public void SetTimeFromFloat(float timeInSeconds)
    {
        this.minutes = Mathf.FloorToInt(timeInSeconds / 60f);

        float remainingSeconds = timeInSeconds - (this.minutes * 60f);
        this.seconds = Mathf.FloorToInt(remainingSeconds);
    }

    public override string ToString()
    {
        string time = null;
        if (minutes < 10)
        {
            time += "0" + minutes;
        }
        else
        {
            time += minutes;
        }

        time += "\"";

        if (seconds < 10)
        {
            time += "0" + seconds;
        }
        else
        {
            time += seconds;
        }

        time += "\'";

        return time;
    }
}
