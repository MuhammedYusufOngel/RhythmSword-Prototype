using UnityEngine;
using RhythmEditor;

public class ChartEditorState
{
    public ChartData chart = new ChartData();
    public AudioClip audioClip;
    public int selectedNoteIndex = -1;
    public float currentTime;
    public float zoomLevel = 100f; // pixels per second
    public float scrollOffset; // time offset in seconds
    public SnapMode snapMode = SnapMode.Off;
    public DIRECTION currentDirection = DIRECTION.LEFT;
    public COLOR currentColor = COLOR.RED;
    public bool isPlaying;
    public float playbackSpeed = 1f;
    public bool isDirty;
    public string currentFilePath = "";
    public ChartNoteData copiedNote;
    public float songDuration;

    public void NewChart()
    {
        chart = new ChartData();
        audioClip = null;
        selectedNoteIndex = -1;
        currentTime = 0f;
        scrollOffset = 0f;
        isPlaying = false;
        isDirty = false;
        currentFilePath = "";
        copiedNote = null;
        songDuration = 0f;
    }

    public bool HasSelection => selectedNoteIndex >= 0 && selectedNoteIndex < chart.notes.Count;
    public ChartNoteData SelectedNote => HasSelection ? chart.notes[selectedNoteIndex] : null;
}
