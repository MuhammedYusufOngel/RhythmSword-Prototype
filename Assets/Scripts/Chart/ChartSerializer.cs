using System;
using System.IO;
using UnityEngine;

public static class ChartSerializer
{
    public static void Save(ChartData chart, string filePath)
    {
        try
        {
            string json = JsonUtility.ToJson(chart, true);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save chart to {filePath}: {e.Message}");
        }
    }

    public static ChartData Load(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<ChartData>(json);
            }
            else
            {
                Debug.LogError($"Chart file not found: {filePath}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load chart from {filePath}: {e.Message}");
            return null;
        }
    }

    public static ChartData LoadFromTextAsset(TextAsset asset)
    {
        if (asset == null)
        {
            Debug.LogError("TextAsset is null. Cannot load chart.");
            return null;
        }
        
        try
        {
            return JsonUtility.FromJson<ChartData>(asset.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse chart from TextAsset {asset.name}: {e.Message}");
            return null;
        }
    }
}
