using System;
using System.Collections.Generic;

[System.Serializable]
public class ChartData
{
    public string songName = "Untitled";
    public float bpm = 120f;
    public string audioClipPath = ""; // relative path within Assets
    public List<ChartNoteData> notes = new List<ChartNoteData>();

    public void SortNotesByTime()
    {
        notes.Sort((a, b) => a.time.CompareTo(b.time));
    }
}
