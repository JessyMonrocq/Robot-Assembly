using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveEntry
{
    public string requestName;
    public string satisfaction;
}

[Serializable]
public class SaveFile
{
    public List<SaveEntry> entries = new List<SaveEntry>();
}

public class SaveData
{
    private static SaveFile saved = null;
    private static string filePath => Path.Combine(Application.persistentDataPath, "save_requests.json");

    private static void EnsureLoaded()
    {
        if (saved != null)
        {
            return;
        }

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                saved = JsonUtility.FromJson<SaveFile>(json) ?? new SaveFile();
            }
            catch
            {
                saved = new SaveFile();
            }
        }
        else
        {
            saved = new SaveFile();
        }
    }

    public static void SetRequestSatisfaction(string requestName, string satisfaction)
    {
        EnsureLoaded();
        var entry = saved.entries.Find(e => e.requestName == requestName);
        if (entry != null)
        {
            entry.satisfaction = satisfaction;
        }
        else
        {
            saved.entries.Add(new SaveEntry { requestName = requestName, satisfaction = satisfaction });
        }
    }

    public static void SetRequestSatisfactionIfHigher(string requestName, string newSatisfaction, SatisfactionLevelsSO satisfactionLevelsSO)
    {
        EnsureLoaded();

        var entry = saved.entries.Find(e => e.requestName == requestName);
        if (entry == null)
        {
            saved.entries.Add(new SaveEntry { requestName = requestName, satisfaction = newSatisfaction });
            return;
        }

        if (satisfactionLevelsSO == null)
        {
            entry.satisfaction = newSatisfaction;
            return;
        }

        if (!satisfactionLevelsSO.TryGetByName(entry.satisfaction, out SatisfactionLevel oldSL))
        {
            entry.satisfaction = newSatisfaction;
            return;
        }

        if (!satisfactionLevelsSO.TryGetByName(newSatisfaction, out SatisfactionLevel newSL))
        {
            return;
        }

        if ((int)newSL.satisfactionDegree > (int)oldSL.satisfactionDegree)
        {
            entry.satisfaction = newSatisfaction;
        }
    }

    public static string GetRequestSatisfaction(string requestName)
    {
        EnsureLoaded();
        var entry = saved.entries.Find(e => e.requestName == requestName);
        return entry?.satisfaction;
    }

    public static void SaveToFile()
    {
        EnsureLoaded();
        try
        {
            string json = JsonUtility.ToJson(saved, true);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save request data: " + e);
        }
    }

    public static void ResetAll()
    {
        saved = new SaveFile();

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete save file: " + e);
        }
    }
}
