using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class PlaytestLogger : MonoBehaviour
{
    public static PlaytestLogger Instance { get; private set; }

    [Header("Playtest Logger Settings")]
    [Tooltip("When true the logger will start logging as soon as the game starts.")]
    public bool enableOnStart = false;

    [Tooltip("Directory where logs will be written (relative to persistentDataPath).")]
    public string relativeLogDirectory = "PlaytestLogs";

    private string sessionFilePath;
    private bool isEnabled = false;

    private string currentRequestName;
    private float assemblingStartTime;
    private float assemblingEndTime;
    private float miniStartTime;
    private float miniEndTime;
    private RobotResult currentRobotResult;
    private bool currentSuccess;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (enableOnStart)
        {
            EnableLogging();
        }
    }

    public void EnableLogging()
    {
        if (isEnabled) return;
        isEnabled = true;
        CreateSessionFile();
        Debug.Log("[PlaytestLogger] Enabled. Logging to: " + sessionFilePath);
    }

    public void DisableLogging()
    {
        isEnabled = false;
    }

    private void CreateSessionFile()
    {
        string dir = Path.Combine(Application.persistentDataPath, relativeLogDirectory);
        try
        {
            Directory.CreateDirectory(dir);
        }
        catch (Exception e)
        {
            Debug.LogWarning("[PlaytestLogger] Could not create log directory: " + e.Message);
        }

        string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        sessionFilePath = Path.Combine(dir, $"playtest_log_{timeStamp}.csv");

        var header = "RequestName,AssemblingTimeSec,MiniGameTimeSec,FinalSatisfaction\n";
        File.WriteAllText(sessionFilePath, header, Encoding.UTF8);
    }

    public void OnRequestStarted(RequestSO request)
    {
        if (!isEnabled) return;
        if (request == null) return;

        currentRequestName = request.name;
        assemblingStartTime = Time.realtimeSinceStartup;
        assemblingEndTime = 0f;
        miniStartTime = 0f;
        miniEndTime = 0f;
        currentRobotResult = null;
        currentSuccess = false;

        Debug.Log($"[PlaytestLogger] Request started: {currentRequestName} at {assemblingStartTime}");
    }

    public void OnRequestFinished(RobotResult results, bool success)
    {
        if (!isEnabled) return;

        assemblingEndTime = Time.realtimeSinceStartup;
        currentRobotResult = results;
        currentSuccess = success;

        Debug.Log($"[PlaytestLogger] Request finished: {currentRequestName} success={success} at {assemblingEndTime}");

        if (!success)
        {
            WriteEntry(isFailure: true);
            ClearCurrentState();
        }
    }

    public void OnMiniGameStarted()
    {
        if (!isEnabled) return;
        if (miniStartTime <= 0f)
        {
            miniStartTime = Time.realtimeSinceStartup;
            Debug.Log($"[PlaytestLogger] MiniGame started at {miniStartTime} for request {currentRequestName}");
        }
    }

    public void OnMiniGameFinished(string finalSatisfactionFromResults = null)
    {
        if (!isEnabled) return;
        miniEndTime = Time.realtimeSinceStartup;
        Debug.Log($"[PlaytestLogger] MiniGame finished at {miniEndTime} for request {currentRequestName}");

        WriteEntry(isFailure: false, finalSatisfactionOverride: finalSatisfactionFromResults);
        ClearCurrentState();
    }

    private void ClearCurrentState()
    {
        currentRequestName = null;
        assemblingStartTime = assemblingEndTime = miniStartTime = miniEndTime = 0f;
        currentRobotResult = null;
        currentSuccess = false;
    }

    private void WriteEntry(bool isFailure, string finalSatisfactionOverride = null)
    {
        if (string.IsNullOrEmpty(sessionFilePath))
        {
            CreateSessionFile();
        }

        string requestName = currentRequestName ?? "Unknown";
        string assemblingTimeStr = "";
        string miniTimeStr = "";
        string finalSatisfaction = "failure";

        if (isFailure)
        {
            if (assemblingEndTime > 0f && assemblingStartTime > 0f)
            {
                int assemblingSeconds = Mathf.RoundToInt(assemblingEndTime - assemblingStartTime);
                assemblingTimeStr = assemblingSeconds.ToString(CultureInfo.InvariantCulture);
            }
            miniTimeStr = "";
            finalSatisfaction = finalSatisfactionOverride ?? "failure";
        }
        else
        {
            if (assemblingEndTime > 0f && assemblingStartTime > 0f)
            {
                int assemblingSeconds = Mathf.RoundToInt(assemblingEndTime - assemblingStartTime);
                assemblingTimeStr = assemblingSeconds.ToString(CultureInfo.InvariantCulture);
            }

            float effectiveMiniStart = miniStartTime > 0f ? miniStartTime : (assemblingEndTime > 0f ? assemblingEndTime : 0f);
            if (miniEndTime > 0f && effectiveMiniStart > 0f)
            {
                int miniSeconds = Mathf.RoundToInt(miniEndTime - effectiveMiniStart);
                miniTimeStr = miniSeconds.ToString(CultureInfo.InvariantCulture);
            }

            finalSatisfaction = finalSatisfactionOverride ?? DetermineFinalSatisfaction(currentRobotResult);
        }

        string line = $"{EscapeCsv(requestName)},{assemblingTimeStr},{miniTimeStr},{EscapeCsv(finalSatisfaction)}\n";

        try
        {
            File.AppendAllText(sessionFilePath, line, Encoding.UTF8);
            Debug.Log("[PlaytestLogger] Logged: " + line.Trim());
        }
        catch (Exception e)
        {
            Debug.LogWarning("[PlaytestLogger] Could not write log: " + e.Message);
        }
    }

    private string EscapeCsv(string s)
    {
        if (s == null) return "";
        if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
        {
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }
        return s;
    }

    private string DetermineFinalSatisfaction(RobotResult result)
    {
        if (result == null)
        {
            return "N/A";
        }

        int count = 0;
        int sumDegrees = 0;

        foreach (var (_, stat) in result.EnumerateStats())
        {
            if (stat.Required <= 0f)
            {
                continue;
            }

            count++;

            if (stat.SatisfactionDegree == SatisfactionLevel.SatisfactionDegree.Unsatisfied)
            {
                return SatisfactionLevel.SatisfactionDegree.Unsatisfied.ToString();
            }

            sumDegrees += (int)stat.SatisfactionDegree;
        }

        if (count == 0)
        {
            return "N/A";
        }

        float avg = (float)sumDegrees / count;
        int nearest = Mathf.Clamp(Mathf.RoundToInt(avg), (int)SatisfactionLevel.SatisfactionDegree.Unsatisfied, (int)SatisfactionLevel.SatisfactionDegree.Perfect);
        var averagedDegree = (SatisfactionLevel.SatisfactionDegree)nearest;

        return averagedDegree switch
        {
            SatisfactionLevel.SatisfactionDegree.Unsatisfied => "Unsatisfied",
            SatisfactionLevel.SatisfactionDegree.Poor => "Bad",
            SatisfactionLevel.SatisfactionDegree.Average => "Average",
            SatisfactionLevel.SatisfactionDegree.Good => "Good",
            SatisfactionLevel.SatisfactionDegree.Perfect => "Perfect",
            _ => averagedDegree.ToString()
        };
    }
}